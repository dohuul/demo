import sys
import time
sys.path.insert(0, "python/utility")
#sys.path.insert(0, "C:\\User\\sqsbtt\\Documents\\jamesle\\github_ssh\\demo\\python\\common\\drivers")
#sys.path.append('../../')

import jquerywizard
#from jquerywizard import init

browser = jquerywizard.init("chrome")
page = browser.new_page()
page.goto("http://playwright.dev")
time.sleep(10)
print(page.title())
browser.close()