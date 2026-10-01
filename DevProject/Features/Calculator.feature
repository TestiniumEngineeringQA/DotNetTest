Feature: Calculator
 
@Deneme
Scenario: Demo

	* Wake up the driver
	* Go to https://www.kitapyurdu.com/index.php?route=account/login Url
	* Wait 3 seconds

@Deneme2
Scenario: Demo2

	* Wake up the driver
	* Go to https://www.kitapyurdu.com/index.php?route=account/login Url
	* Wait 3 seconds

@FailDemo
Scenario: FailDemo
	* Wake up the driver
	* Go to https://www.kitapyurdu.com/index.php?route=account/login Url
	* Wait 2 seconds
	* Click "#login-button"
