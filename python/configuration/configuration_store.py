import json
import sys
import os

class ConfigurationStore:
    

    def get_credential(credential_name):
        dir_path = os.path.abspath(os.path.dirname(__file__))
        my_path = dir_path + "/configuration_store.json"
       
        print("path in config" + my_path)
       
        CONFIG_STORE = json.load(open(my_path))

        print(CONFIG_STORE["targetEnvironment"])
      
        return CONFIG_STORE["targetEnvironment"]
    
    def get_product_url(product_name):
        return ""
    
    def get_service_url_live(service_name):
        return ""
    
    def get_service_url_non_live(service_name):
        return ""

    def get_app_config(config_name):
        return {
            "key1":"value1",
            "key2":"value2"
        }