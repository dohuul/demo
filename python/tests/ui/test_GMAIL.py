import sys
import time

import python.utility.selenium_utilities as SeleniumUtilities


from selenium import webdriver
from selenium.webdriver.chrome.service import Service as ChromeService
from webdriver_manager.chrome import ChromeDriverManager
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
 


#import style #1 insert path into the directory where intepreter will search for module 
sys.path.insert(0, "python/utility")
import playwright_utilities

#import style #2 absolute path
import python.configuration.configuration_store as CONFIG

from python.utility.xml_utilities import Tree
from python.utility.xml_utilities import Banyan
from python.utility.xml_utilities import Peepal





# This test is designed to fail for demonstration purposes.
def test_GMAIL_login_sanity_PLAYWRIGHT():
    gmail_ui_url = CONFIG.ConfigurationStore.get_product_url("gmail_ui")
    browser = playwright_utilities.PlayWrightUtilities.init("chrome")
    page = browser.new_page()
    page.goto(gmail_ui_url)
    time.sleep(10)
    print(page.title())
    browser.close()


def test_GMAIL_login_sanity_SELENIUM():
    gmail_ui_url = CONFIG.ConfigurationStore.get_product_url("gmail_ui")
    driver = SeleniumUtilities.SeleniumUtilites.init("chrome")
    driver.get(gmail_ui_url)
    we_user  = driver.find_element(By.ID, "identifierId")
    driver.find_element
    we_user.send_keys("dohuul")
    time.sleep(10)
    driver.close()
    driver.quit()
   


def test_positive():   
   test_sign_up_successful("jamesle_test", "dohuul@gmail.com", "dohuul@gmail.com", "jamesle_test")

def test_positive_boundary_email():   
   test_sign_up_successful("jamesle_test", "this_is_equal_to_40_1111111111@gmail.com", "this_is_equal_to_40_1111111111@gmail.com", "jamesle_test")

def test_negative():
     test_sign_up_bad_input_username("j", "dohuul@gmail.com", "dohuul@gmail.com")

def test_negative_boundary_email():   
   test_sign_up_bad_input_email("jamesle_test", "this_is_greater_than_40_11111111@gmail.com", "this_is_greater_than_40_11111111@gmail.com")

    



def test_sign_up_successful(input_username, input_email1,input_email2, test_expected_text):
    user_name = input_username
    email1 = input_email1
    email2 = input_email2
    expected_text = test_expected_text
    wait_in_seconds = 20
    gmail_ui_url = "https://assessments.reliscore.com/base/ssubscribe1/"
    driver = webdriver.Chrome(service=ChromeService(ChromeDriverManager().install()))

    try:
        driver.get(gmail_ui_url)
        element_username = WebDriverWait(driver, wait_in_seconds).until(EC.presence_of_element_located((By.ID, "id_username")))
        element_username.send_keys(user_name)
        element_email1 = WebDriverWait(driver, wait_in_seconds).until(EC.presence_of_element_located((By.ID, "id_email1")))
        element_email1.send_keys(email1)
        element_email2 = WebDriverWait(driver, wait_in_seconds).until(EC.presence_of_element_located((By.ID, "id_email2")))
        element_email2.send_keys(email2)
        element_button = WebDriverWait(driver, wait_in_seconds).until(EC.presence_of_element_located((By.XPATH, '//input[@value="Subscribe"]')))
        element_button.click()

        element_success = WebDriverWait(driver, wait_in_seconds).until(EC.presence_of_element_located((By.ID, "signup-success")))
        element_success_text = element_success.get_attribute('innerHTML')
        assert element_success_text.index("You have successfully")  > 0
        assert element_success_text.index("subscribed with username")  > 0
        assert element_success_text.index(expected_text)  > 0
    except:
        raise Exception("test failure - run debug to troubleshoot")
    finally:
        time.sleep(2)
        driver.close()
        driver.quit()

def test_sign_up_bad_input_username(input_username, input_email1,input_email2):
    user_name = input_username
    email1 = input_email1
    email2 = input_email2
   
    wait_in_seconds = 20
    gmail_ui_url = "https://assessments.reliscore.com/base/ssubscribe1/"
    driver = webdriver.Chrome(service=ChromeService(ChromeDriverManager().install()))

    try:
        driver.get(gmail_ui_url)
        element_username = WebDriverWait(driver, wait_in_seconds).until(EC.presence_of_element_located((By.ID, "id_username")))
        element_username.send_keys(user_name)
        element_email1 = WebDriverWait(driver, wait_in_seconds).until(EC.presence_of_element_located((By.ID, "id_email1")))
        element_email1.send_keys(email1)
        element_email2 = WebDriverWait(driver, wait_in_seconds).until(EC.presence_of_element_located((By.ID, "id_email2")))
        element_email2.send_keys(email2)
        element_button = WebDriverWait(driver, wait_in_seconds).until(EC.presence_of_element_located((By.XPATH, '//input[@value="Subscribe"]')))
        element_button.click()

        element_failure = WebDriverWait(driver, wait_in_seconds).until(EC.presence_of_element_located((By.ID, "error_1_id_username")))
        element_failure_text = element_failure.get_attribute('innerHTML')
        assert element_failure_text.index("Username must be 5 or more characters")  > 0
        
    except:
        raise Exception("test failure - run debug to troubleshoot")
    finally:
        time.sleep(2)
        driver.close()
        driver.quit()
        


def test_sign_up_bad_input_email(input_username, input_email1,input_email2):
    user_name = input_username
    email1 = input_email1
    email2 = input_email2
   
    wait_in_seconds = 20
    gmail_ui_url = "https://assessments.reliscore.com/base/ssubscribe1/"
    driver = webdriver.Chrome(service=ChromeService(ChromeDriverManager().install()))

    try:
        driver.get(gmail_ui_url)
        element_username = WebDriverWait(driver, wait_in_seconds).until(EC.presence_of_element_located((By.ID, "id_username")))
        element_username.send_keys(user_name)
        element_email1 = WebDriverWait(driver, wait_in_seconds).until(EC.presence_of_element_located((By.ID, "id_email1")))
        element_email1.send_keys(email1)
        element_email2 = WebDriverWait(driver, wait_in_seconds).until(EC.presence_of_element_located((By.ID, "id_email2")))
        element_email2.send_keys(email2)
        element_button = WebDriverWait(driver, wait_in_seconds).until(EC.presence_of_element_located((By.XPATH, '//input[@value="Subscribe"]')))
        element_button.click()

        element_failure = WebDriverWait(driver, wait_in_seconds).until(EC.presence_of_element_located((By.ID, "error_1_id_email1")))
        element_failure_text = element_failure.get_attribute('innerHTML')
        assert element_failure_text.index("Email length must be 40 or less")  > 0
        
    except:
        raise Exception("test failure - run debug to troubleshoot")
    finally:
        time.sleep(2)
        driver.close()
        driver.quit()
        