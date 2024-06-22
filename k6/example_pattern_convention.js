import http from 'k6/http';
import {configuration} from './configuration.js';
import {Trend} from 'k6/metrics';
import {group,check} from 'k6';

/*  read me 
    design naming and convetion
        -design naming and convention based on application under test so that test can closely relate to the resouces of application under test
        -utilize k6 core class/functions first before using external (allow us to understand the k6 core class throughly)
        -create a common test flow/nameing/convetion and other tests need to follow so all tests have same pattern (easier to understand, easier to read, easier to make changes)
        -always put documentation, something is better than nothing.
    script flow:
        -init section: anything outside of setUp(), teardown(), and main()
        -setUp(): place things like warmup logic, seed mock data logic, get login token logic, etc...
        -tearDown(): place things like database connection close, delete mock data logic, web hook reporting logic, etc...
        -only these above places can call global variable. Ohter functions should not use global variable.

    environment:
        -check _configuration.js for all supported environment

    nonlive test:

    commandline example
        -example 1: set "HTTPS_PROXY=localhost:8888" && k6 run example_pattern_convention.js  -e thread=2 -e duration=1m
*/

/* input  from commandline */
const environment_string = __ENV.environment;
const thread = __ENV.thread;
const duration = __ENV.duration;
const is_public = __ENV.is_public;
const is_nonlive = __ENV.is_nonlive;
const is_spike_test = __ENV.is_spike;
let http_timeout = '60s';

/* input validation and throw error to user */
if(thread === undefined || isNaN(thread)){
    throw `error: unsupported value for thread parameter. Thread parameter should be a number`.
}

let nThread = Number(thread);
if(!Number.isInteger(nThread)){
    throw `error: unsupported value for thread parameter. Thread parameter should be an integer`;
}

let iThread = parseInt(nThread);
if(iThread < 1 || iThread > 100){
    throw `error: unsupported value for thread parameter. Thread parameter should be between 1 and 100`;
}


/*service name , version, gateway and base url */
let gateway_name = 'api_gateway';
const version = 'v2';
const zone_name = 'lnj';
let url_BASE = `${configuraion[environment_string][gateway_name]}`;

/* resolve public and nonlive */
gateway_name = is_public === 'true' ? 'dgw_public' : gateway_name;
url_BASE = is_nonlive === 'true' ? url_BASE.replace('https://', `https://${configuraion["dgw_nonlive_prefix"]}.`) : url_BASE;


/* create service url based on application under test */
const url_ORDER = `${url_BASE}/${version}/${zone_name}/order`;
const url_REPORT = `${url_BASE}/${version}/${zone_name}/report?transactionID`;
const url_MOCK = `${url_BASE}/${version}/${zone_name}/mock`;
const url_HEALTH = `${url_BASE}/${version}/${zone_name}/health`;

/* open file */
let mock_vendor_response = read_file_from('/test_data/dgw_api_lnj/prod/path_to_file.txt');

/* create trend
    reason 1: to have individual http call under its own section. otherwise all https call in the main functoin will be reported as one.
*/
let trend_name = 'trend_LNJ_POST';
let trend_name_warmup = 'trend_LNJ_POST_warmup';
const trend_LNJ_POST = new Trend(trend_name);
const trend_LNK_POST_warmup = new Trend(trend_name_warmup);

/* set k6 option
    if not spike test, use fixed thread option, else user spike option
*/
export let options = is_spike_test !== 'true' ? {
    vus: thread,
    insercureSkipTLSVerify:true,
    noConnectionResue:true,
    duration: duration,
    setupTimeout: '3m' //this value should be longer than warmup period
} : 
{
    stages: [
        {duration:'30s', target:2},
        {duration: '1m', target:7}
    ]
}

/* setup
    example: log urls, warmup calls, seed mock, etc...
*/
export function setup(){
    console.log(`url_BASE: ${url_BASE}`);
    console.log(`url_ORDER: ${url_ORDER}`);

    //set expected http status in HTTP Response
    http.setResponseCallback(
        http.expectedStatuses(200, 404)
    );

    group('Warmup before actual test', ()=>{
        const minutesToWarmup = 1;
        const datetimeStart = new Date();

        //loop until warmup is done
        let minutesDifference= 0;
        while(minutesDifference < minutesToWarmup){
            const requestsWarmup = {
                [`${trend_name_warmup}`] : create_request_POST(url_ORDER, http_timeout, 'test', 'test', '999999990');
            }

            let response = http.batch(requestsWarmup);

            trend_LNK_POST_warmup.add(responses[`${trend_LNJ_POST}`].timings.duration);

            let responseBody = responses[`${trend_name_warmup}`].body;

            check(responseBody,{
                'Is warmup up pass' : (r) => responseBody.include("{");
            });

            //get minute difference
            const dateTimeCurrent = new Date();
            minutesDifference = function(dateTimeStart, dateTimeEnd){
                let differenceValue = (dateTimeStart.getTime() - dateTimeEnd.getTime()) / 1000;
                differenceValue /= 60;
                return Math.abs(Math.round(differenceValue));
            }(datetimeStart, dateTimeCurrent);
        }
    });

    group('Seed mock data', ()=>{
        const requestsMock = {
            'requestMock' : create_request_POST_MOCK(url_MOCK, http_timeout, 'test', 'test', 'middle', '999999990', mock_vendor_response)
        };

        let response = http.batch('requestMock').body;

        check(responseBody, {
            "Is mock seed pass": (r) => responseBody.includes("{")
        })
    });
}


export default function main(data) {
    Test_Post_Order(url_POST, http_timeout, 'test', 'test', '', '999999990');
}

export function teardown(data) {

}


/* Test Case */
function Test_Post_Order(serviceUrl, httpTimeout, first, last, middle, ssn){
    group('Test Post Order', () => {
        const requests  = {
            [`${trend_name}`] : create_request_POST(serviceUrl, httpTimeout, first, last, middle, ssn)
        }

        let responses = http.batch(requests);

        trend_LNJ_POST.add(response[`${trend_name}`].timings.duration);

        let responseBody = responses[`${trend_name}`].body;
        
        check(responseBody, {
            'Is responseBody json' : (r) => responseBody.includes("content test")
        })
    });
}


/* Create Request */
function create_request_POST(serviceUrl, serviceTimeout, firstName, lastName, midName, ssn){
    let http_headers = {
        'content-type':'application/json',
        'px-globalid' : 'companyID',
        'px-clientid' : 'clientID',
        'px-portalCode' : 'product or gateway name'
    }

    let internal_url = serviceUrl;

    let post_body = {
        'Person' : {
            'ID' : 1,
            'Name' : {
                'First' : `${firstName}`,
                'LastOrCompany' : `${lastName}`,
                'Middle' : `${midName}`
            },
            'SSN' : `${ssn}`
        },
        'RequestorID': 'random guid'
    }

    let k6_request = {
        method: 'POST',
        url: internal_url,
        params: {
            headers: http_headers,
            timeout: serviceTimeout
        },
        body: JSON.stringify(post_body)
    }

    return k6_request;
}

function create_request_POST_MOCK(serviceUrl, serviceTimeout, firstName, lastName, midName, ssn, mockResponse){
    let http_headers = {
        'content-type':'application/json',
        'px-globalid' : 'companyID',
        'px-clientid' : 'clientID',
        'px-portalCode' : 'product or gateway name'
    }

    let internal_url = serviceUrl;

    let post_body = {
        'MockServiceRequest': {
            'Person' : {
                'ID' : 1,
                'Name' : {
                    'First' : `${firstName}`,
                    'LastOrCompany' : `${lastName}`,
                    'Middle' : `${midName}`
                },
                'SSN' : `${ssn}`
            },
            'RequestorID': 'random guid'
        },
        'MockServiceResponseJson' : mockResponse
    }   

    let k6_request = {
        method: 'POST',
        url: internal_url,
        params: {
            headers: http_headers,
            timeout: serviceTimeout
        },
        body: JSON.stringify(post_body)
    }

    return k6_request;
}



