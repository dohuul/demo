import sys
import time

#import style #1 insert path into the directory where intepreter will search for module 
sys.path.insert(0, "python/utility")
import playwright_utilities

#import style #2 absolute path
import python.configuration.configuration_store as CONFIG

# This test is designed to fail for demonstration purposes.
def test_GMAIL_login_sanity_PLAYWRIGHT():
    gmail_ui_url = CONFIG.ConfigurationStore.get_product_url("gmail_ui")
    browser = playwright_utilities.init("chrome")
    page = browser.new_page()
    page.goto(gmail_ui_url)
    time.sleep(10)
    print(page.title())
    browser.close()