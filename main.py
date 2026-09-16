# required imports
import datetime
import os
import json
from pathlib import Path
import src.NICE.configuration as configuration
import src.NICE.storage as storage
import src.NICE.features as features



# custom imports
from src.NICE.calculator import calc
from src.NICE.calendar import calendar


# Presets
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

def main():
    features.clear()
    print(greeting)
    menuChoice = int(input(menu))
    if menuChoice == 1: calc(); features.clear()
    elif menuChoice == 2: calendar(); features.clear()
    elif menuChoice == 3: print("Thank you for using NICE!"); exit()
    

main()
