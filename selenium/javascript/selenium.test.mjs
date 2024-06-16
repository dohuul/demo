import {describe} from "node:test";
import {Builder, By,until} from "selenium-webdriver";
import { setTimeout } from "timers/promises";

describe("testname1", async () =>{
    await selenium_test();
});



async function selenium_test (){
    
    let use_grid = false;
    let test_url = "https://fraudguard.interthinx.com";
    let grid_url = "http://localhost:4444";
    let grid_url_selenoid = "http://localhost:4444/wd/hub";
    let browser_name = "chrome";
    let wait_time_out = 2000;

    let builder_obj = use_grid ?
    await new Builder().usingServer(grid_url_selenoid).forBrowser(browser_name) :
    await new Builder().forBrowser(browser_name);

    let web_driver = await builder_obj.build();
 
    try {
        console.log(until);
        await web_driver.get(test_url);

        await web_driver.wait(async (wd) => {
            let current_url = await wd.getCurrentUrl();
            return current_url.indexOf('newlogins.interthinx.com') > 0;
        }, wait_time_out);

        let input_username = await web_driver.wait(async (wd) => {
            let element = await wd.findElement(By.id("userNameInput"));
            if(element.isDisplayed){
                return element;
            }                      
        }, wait_time_out);
        await input_username.sendKeys("yentest2");

        let input_password = await web_driver.wait(until.elementLocated(By.id("passwordInput")),wait_time_out);
        await input_password.sendKeys("1Hoolie.");

        let input_button = await web_driver.wait(until.elementLocated(By.id("submitButton"), wait_time_out));
        await web_driver.wait(until.elementIsEnabled(input_button));
        await input_button.click();

        await web_driver.wait(until.urlContains("home.aspx"));

        let input_upload_loan = await web_driver.wait(until.elementLocated(By.id("imgUpload"), wait_time_out));
        await input_upload_loan.click();

        let current_ur2l = await web_driver.getCurrentUrl();
        let current_window_handle = await web_driver.getWindowHandle();
        let window_handles = await web_driver.getAllWindowHandles();
        for(let w_handle in window_handles){
            if(window_handles[w_handle] != current_window_handle){
                await web_driver.switchTo().window(window_handles[w_handle]);
            }
        }
        let current_tiyle = await web_driver.getTitle();
        let current_url = await web_driver.getCurrentUrl();
        await web_driver.wait(until.urlContains("uploadloan.aspx"), wait_time_out);

        let input_file_upload_1 = await web_driver.wait(until.elementLocated(By.id("InputFile1")), wait_time_out);
        await input_file_upload_1.sendKeys("C:\\Users\\daxan\\Downloads\\test.txt");

        await setTimeout(1000);
    }
    catch(e){
        throw e;
    }
    finally{
        await web_driver.quit();
    }

  
}


class BasePage {
    constructor(){
        
    }
}