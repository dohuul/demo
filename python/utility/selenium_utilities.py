from selenium import webdriver
from selenium.webdriver.chrome.service import Service as ChromeService
from webdriver_manager.chrome import ChromeDriverManager

from selenium.webdriver.firefox.service import Service as FirefoxService
from webdriver_manager.firefox import GeckoDriverManager

from webdriver_manager.microsoft import EdgeChromiumDriverManager
from selenium.webdriver.edge.service import Service as EdgeService

class SeleniumUtilites:
    def init(browser_name):     
   
        driver = ""

        if(browser_name == "edge"):
            driver = webdriver.Edge(service=EdgeService(EdgeChromiumDriverManager().install()))
        elif(browser_name == "chrome"):
            driver = webdriver.Chrome(service=ChromeService(ChromeDriverManager().install()))
        elif(browser_name == "firefox"):
           driver = webdriver.Chrome(service=FirefoxService(GeckoDriverManager().install()))
        else:
            raise Exception("Error at SeleniumUtilites| message={message} |browser={browser}".format(message="unsupport browser",browser=browser_name))

        return driver