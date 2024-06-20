//k6 run example_data_driven.js -e environment=rc -e thread=1 -e duration=2m -e data=mock

import http from 'k6/http'
import papaparse from 'https://jslib.k6.io/papaparse/5.1.1/index.js'
import { SharedArray } from 'k6/data';
import {Trend} from 'k6/metrics'
import {uuidv4} from 'https://jslib.k6.io/k6-utils/1.0.0/index.js'
import {describe,expect} from 'https://jslib.k6.io/k6chaijs/4.3.4.1/index.js'

var environment =  {
    "rc":{
        "url_post": "",
        "url_get" :""
    },
    "prod":{
        "url_post": "",
        "url_get" :""
    }
}

//define input arguments
var command_line_environment = __ENV.environment;
var url_post = environment[command_line_environment]["url_post"];
var url_get = environment[command_line_environment]["url_get"];
var thread = __ENV.thread;
var duration = __ENV.duration;
var data = __ENV.data;

//define options
let regular_options = {
    vus: thread,
    insecureSkipTLSVerify:true,
    noConnectionReuse:true,
    duration: duration
}

let spike_options = {
    insecureSkipTLSVerify:true,
    noConnectionReuse:true,
    stages: [
        {duration: '60s', target:10},
        {duration: '20s', target:40},
        {duration: '60s', target:10}     
    ]
}

let final_option = thread == 'spike' ? spike_options : regular_options;
export let options = final_option;

//define data
let data_file_name = data == 'mock' ? '/Test_Data/Addresses/mock_30K.csv' : '/Test_Data/Addresses/prod_addresses_30K.csv';
let csvData = new SharedArray("some name here", function() {
    return papaparse.parse(open(data_file_name), {header:true}).data;
});


//create request
function create_post_request (url, address_line, city,state,zip){
     let k6_options = {
        headers: {
            'content-type':'application/json',
            'cacheperiod': '-1',
            'px-clientid': 'yentest2',
            'px-globalid': 'slinger1',
            'px-portalcode': 'api'
        },
        timeout: '60s'
     }

     let post_body = {
        'RequestorID': 'load-test' + uuidv4().substring(1,15);
        'Address' : {
            'StreetAddress1' : address_line,
            'City' : city,
            'State' : state,
            'ZipCode' : zip
        }
     }

     let k6_request = {
        method: 'POST',
        url:url,
        params: k6_options,
        body: JSON.stringify(post_body)
     };
     return k6_request;
}

function create_get_request(baseurl, transid){
    let k6_options = {
        headers: {
            'content-type':'application/json',
            'cacheperiod': '-1',
            'px-clientid': 'yentest2',
            'px-globalid': 'slinger1',
            'px-portalcode': 'api'
        },
        timeout: '60s'
     }

     let url = baseurl + transid;

     let k6_request = {
        method: 'GET',
        url :url,
        params : k6_options
     }

     return k6_request;
}

//create trend
const post_trend = new Trend('post_trend');
const get_trend = new Trend('get_trend');

//create counter
let iThread = parseInt(thread);
let counter = 0;
export default function(data){

    let item_index = __VU + counter;
    let street = csvData[item_index].street;
    let city = csvData[item_index].city;
    let state = csvData[item_index].state;
    let zip = csvData[item_index].zip;

    counter = counter + iThread;
    if(counter + iThread > csvData.length - 2){
        counter = 0;
    }

    let requests = {
        "post_test" : create_post_request(url_post, street, city, state, zip);
    }
    let responses = http.batch(requests);
    describe('post response check', ()=>{
        let s_body = response["post_test"].body;
        expect(response["post_test"].body).to.include('TransactionID');
    });

    let post_body_json = JSON.parse(responses["post_test"].body);
    let transaction_id = post_body_json.TransactionID;

    let requests_get = {
        "get_test" : create_get_request(url_get, transaction_id);
    }

    let responses_get = http.batch(requests_get);

    post_trend.add(responses["post_test"].timings.duration);
    get_trend.add(responses_get["get_test"].timings.duration);

  
}