import requests

class RESTUtilities:

    def execute_web_request(url, http_action, headers, data):
        response = None
        try:
            if(data == None):
                response = requests.request(http_action, url)
            elif((data.startswith("{") and data.endswith("{")) or
            (data.startswith("[") and data.endswith("]"))):
                response = requests.request(http_action, url, headers=headers, json=data)
            else:
                response = requests.request(http_action, url, headers=headers, data=data)

            response.raise_for_status()
            
        except requests.exceptions.Timeout:
            response = None
          
        return response

    def execute_GET(url, headers, retry_count = 0):
        return RESTUtilities.execute_web_request(url, "GET", headers, None)
        
  