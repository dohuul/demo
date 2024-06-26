import json
import python.utility.rest_utilities as RESTClient
import python.configuration.configuration_store as CONFIG
import python.utility.json_utilities as JSONUtilities

def test_Sanity_Get_Pizza():
    service_url = CONFIG.ConfigurationStore.get_service_url_live("pizza_service")
    response = RESTClient.RESTUtilities.execute_GET(service_url, None)
    
    assert response.status_code == 200, "Expect status={expect}. Actual status={actual}".format(expect=200,actual=response.status_code)
    response_text = response.text
    
    is_json_content = JSONUtilities.JSONUtilities.is_valid_json(response_text)
    assert is_json_content == True, "Expect json content={expect}. Actual json content={actual}".format(expect=True,actual=is_json_content)

    json_document = json.loads(response_text)
    item_length = len(json_document)
    assert item_length > 4, "Expect JSON array count={expect}.Actual count={actual}".format(expect=4,actual=item_length)

    response_sanity_value = json_document[0]["id"]
    assert response_sanity_value == "1", "Expect json element value={expect}.Actual value={actual}".format(expect=1, actual=response_sanity_value)

def test_Sanity_POST_Pizza():
    service_url = CONFIG.ConfigurationStore.get_service_url_live("pizza_service")
    post_data = '{ "requestId": "$datatype.uuid", "items": "$mockData", "count": "$count", "anyKey": "anyValue" }'
    post_header = {
        "content-type":"application/json"
    }
    response = RESTClient.RESTUtilities.execute_POST(service_url,post_header, post_data, 0 )
    assert response.status_code == 201, "Expect status={expect}. Actual status={actual}".format(expect=201,actual=response.status_code)
