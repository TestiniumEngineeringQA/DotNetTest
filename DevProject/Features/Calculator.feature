Feature: Calculator
@Deneme
Scenario: Demo

	* Wake up the driver
	* Go to https://www.kitapyurdu.com/index.php?route=account/login Url
	* Wait 3 seconds


@FailDemo
Scenario: FailDemo
	* Wake up the driver
	* Go to https://www.kitapyurdu.com/index.php?route=account/login Url
	* Wait 2 seconds
	* Click "#login-button"