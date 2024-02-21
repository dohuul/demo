import sys
import time
import os

sys.path.insert(0, "python/utility")
import playwright_utilities

sys.path.insert(0, "python/configuration")
import configuration_store




def test_CONFIG_STORE_sanity():
    print("hello")
    dir_path = os.path.dirname(os.path.realpath(__file__))
    print("path" + dir_path)
    configuration_store.ConfigurationStore.get_credential("tests")
    assert False

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