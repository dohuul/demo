#import sys
#sys.path.insert(0, "python/configuration")
from python.configuration.configuration_store import ConfigurationStore

def test_CONFIG_STORE_Credentail_Get():    
    credentials = ConfigurationStore.get_credential("TestID1")
    assert credentials[0] == "test123" , "credential id should be test123"
    assert credentials[1] == "test123pass", "credential pass should be test123pass"