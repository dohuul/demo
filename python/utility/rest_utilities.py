import requests
use_fiddler_proxy_and_ignore_ssl_error = True
class RESTUtilities:

    def execute_web_request(url, http_action, headers, data):     
        verify_ssl_error = False if use_fiddler_proxy_and_ignore_ssl_error == True else True

        proxies = {
            "http" : "127.0.0.1:8888",
            "https" : "127.0.0.1:8888"          
        } if use_fiddler_proxy_and_ignore_ssl_error == True else None

        response = None
        try:
            if(data == None):
                response = requests.request(http_action, url,proxies=proxies, verify=verify_ssl_error)
            elif((data.startswith("{") and data.endswith("{")) or
                (data.startswith("[") and data.endswith("]"))):
                response = requests.request(http_action, url, headers=headers, proxies=proxies, json=data, verify=verify_ssl_error)
            else:
                response = requests.request(http_action, url, headers=headers, proxies=proxies, data=data, verify=verify_ssl_error)

            response.raise_for_status()
            
        except requests.exceptions.Timeout:
            response = None
          
        return response

    def execute_GET(url, headers, retry_count = 0):
        return RESTUtilities.execute_web_request(url, "GET", headers, None)
    
    def execute_POST(url, headers, data, retry_count = 0):
        return RESTUtilities.execute_web_request(url, "POST", headers, data)
        
  