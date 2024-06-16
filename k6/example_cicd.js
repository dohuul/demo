//define reference to external library
import http from 'k6/http';
import {sleep, check} from 'k6';
import encoding from 'k6/encoding';
import {describe} from 'https://jslib.k6.io/expect/0.0.4/index.js';
import {parseHTML} from 'k6/html';

//define script parameter or script interface or script hook to outside worlds or hook to cicd or public client
let zone_Code = __ENV.zoneCode.toLocaleLowerCase();
let cicd_region = __ENV.cicdRegion.toLocaleLowerCase();
let cicd_env = __ENV.cicdEnv.toLocaleLowerCase();
let cicd_color = __ENV.cicdColor.toLocaleLowerCase();
let is_non_live = __ENV.isNonLive.toLocaleLowerCase();

//define default time out for API calls
let default_time_out = '60s';

//define K6 option
export const options = {
    vus:1,
    duration:'1m',
    threshold: {
        http_req_failed : ["rate < 0.01"],
        http_req_duration: ["p(95) < 25000"],
        checks: ['rate > 0.98']
    }

}

//define main test logic, K6 entry point
export default function main(data){


}


class Odata_Utilities {

    static call_odata_GetTrackingByTrackingID(trackingid, cicd_region, cicd_env){
        let test_name = 'call_odata_GetTrackingByTrackingID';
    }

    static is_odata_response_bad(http_web_response, response_name){
        let is_bad = false;
        if(http_web_response === null || http_web_response === undefined){
            console.log(`${response_name}_is_ODATA_http_web_response_bad_at-step-1-error: http web response is null`);
            is_bad = true;
        }
        else if (http_web_response.status !== 200){
            console.log(`${response_name}_is_ODATA_http_web_response_bad_at-step-2-error: http web response status is ${http_web_response.status}`);
            is_bad = true;
        }    
        else if (http_web_response.body === null || http_web_response.body === undefined){
            console.log(`${response_name}_is_ODATA_http_web_response_bad_at-step-3-error: http web response body is null`);
            is_bad = true;
        }
        else if (http_web_response.body.startsWith('<') === false 
              && http_web_response.body.startsWith('{') === false
              && http_web_response.body.startsWith('[') === false){
            console.log(`${response_name}_is_ODATA_http_web_response_bad_at-step-4-error: http web response body is not json or xml`);
            is_bad = true;
        }
        else{
            is_bad = false;
        }
        return is_bad;
    }
}



//Retry utilities
class Retry_Utilities {

    static retry(retry_max_count, retry_sleep_in_second, function_to_retry_name, function_to_retry){

        let current_retry_count= 0;
        let error_message = '';

        while(current_retry_count < retry_max_count){
            try{
                function_to_retry();
                error_message = '';
                break;
            }
            catch(error){
                sleep(retry_sleep_in_second);
                error_message = error;
            }
        }

        if(error_message !== ''){
            throw error_message
        }

    }

}