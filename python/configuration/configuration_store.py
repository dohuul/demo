import json
import os

#wrap config file inside a class
#advantage: file structure change does not affect client code
#disadvantage: more complex than parsing config file from client code
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
        product_url = ConfigurationStore.CONFIG_STORE["environments"][ConfigurationStore.TARGET_ENVIRONMENT]["Product"][product_name]["url"]
        return product_url
    
    def get_service_url_live(service_name):
        service_url = ConfigurationStore.CONFIG_STORE["environments"][ConfigurationStore.TARGET_ENVIRONMENT]["Service"][service_name]["url_live"]
        return service_url
    
    def get_service_url_non_live(service_name):
        service_url = ConfigurationStore.CONFIG_STORE["environments"][ConfigurationStore.TARGET_ENVIRONMENT]["Service"][service_name]["url_nonlive"]
        return service_url

    def get_app_config(config_name):
        return {
            "key1":"value1",
            "key2":"value2"
        }