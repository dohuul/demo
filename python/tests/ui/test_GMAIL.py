import sys
import time
import os

sys.path.insert(0, "python/utility")
import playwright_utilities

sys.path.insert(0, "python/configuration")
import configuration_store




def test_CONFIG_STORE_Credentail_Get():    
    credentials = configuration_store.ConfigurationStore.get_credential("TestID1")
    assert credentials[0] == "test123" , "credential id should be test123"
    assert credentials[1] == "test123pass", "credential pass should be test123pass"

def test_HELLO_sanity():
    assert 4 == 4

# This test is designed to fail for demonstration purposes.
def test_GMAIL_login_sanity():
    browser = playwright_utilities.init("chrome")
    page = browser.new_page()
    page.goto("http://playwright.dev")
    time.sleep(10)
    print(page.title())
    browser.close()