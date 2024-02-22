import json
import os

class ConfigurationStore:
    CONFIG_STORE = json.load(open( os.path.abspath(os.path.dirname(__file__)) 
                                  + "/configuration_store.json"))
    TARGET_ENVIRONMENT = CONFIG_STORE["targetEnvironment"]

    def get_credential( credential_name):             
        credentials = []
        credentials.append(ConfigurationStore.CONFIG_STORE["environments"]
              [ConfigurationStore.TARGET_ENVIRONMENT]["Credential"][credential_name]["ID"])
        credentials.append(ConfigurationStore.CONFIG_STORE["environments"]
              [ConfigurationStore.TARGET_ENVIRONMENT]["Credential"][credential_name]["Pass"])
        
        return credentials
    
    def get_product_url(product_name):
        return ""
    
    def get_service_url_live(service_name):
        service_url = ConfigurationStore.CONFIG_STORE["environments"][ConfigurationStore.TARGET_ENVIRONMENT]["Service"][service_name]["url_live"]
        return service_url
    
    def get_service_url_non_live(service_name):
        return ""

    def get_app_config(config_name):
        return {
            "key1":"value1",
            "key2":"value2"
        }