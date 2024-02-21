import sys
import time

sys.path.insert(0, "python/utility")
import playwright_utilities

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