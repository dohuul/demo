from playwright.sync_api import sync_playwright

def init(browser_name):
    
    #start playwright
    playwright = sync_playwright().start()

    #headleass
    isHeadLess = False

    browser = ""
    if(browser_name == "edge"):
        browser = playwright.chromium.launch(channel="msedge", headless=isHeadLess)
    elif(browser_name == "chrome"):
        browser = playwright.chromium.launch(channel="chrome", headless=isHeadLess)
    elif(browser_name == "firefox"):
        browser = playwright.firefox.launch(headless=isHeadLess)
    
    print("test")
    return browser


