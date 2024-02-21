from playwright.sync_api import sync_playwright

def init(browser_name):   
    
    playwright = sync_playwright().start()  
    
    isHeadLess = False

    browser = ""
    if(browser_name == "edge"):
        browser = playwright.chromium.launch(channel="msedge", headless=isHeadLess)
    elif(browser_name == "chrome"):
        browser = playwright.chromium.launch(channel="chrome", headless=isHeadLess)
    elif(browser_name == "firefox"):
        browser = playwright.firefox.launch(headless=isHeadLess)
    
    #todo 
    #add jquery reference
    #set default paramters for each browser

    print("test")
    return browser


