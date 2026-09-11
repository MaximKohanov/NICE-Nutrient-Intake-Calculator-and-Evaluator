from calculator import calc

def main():

    greeting = r"""
                                    
                .-.                     
     ___ .-.   ( __)   .--.      .--.   
    (   )   \  (''")  /    \    /    \  
     |  .-. .   | |  |  .-. ;  |  .-. ; 
     | |  | |   | |  |  |(___) |  | | | 
     | |  | |   | |  |  |      |  |/  | 
     | |  | |   | |  |  | ___  |  ' _.' 
     | |  | |   | |  |  '(   ) |  .'.-. 
     | |  | |   | |  '  `-' |  '  `-' / 
    (___)(___) (___)  `.__,'    `.__.'  
                                    
    Welcome to NICE (Nutrient Intake Calculator and Evaluator)!"
                                    """

    menu = r"""
    Please select an option from the menu below:
    1. Calculator
    2. Calendar
    3. Exit
    """
    print(greeting)
    menuChoice = int(input(menu))
    if menuChoice == 1: calc()
    elif menuChoice == 2: print("Calendar feature coming soon!")
    elif menuChoice == 3: print("Thank you for using NICE!"); exit()
    

main()
