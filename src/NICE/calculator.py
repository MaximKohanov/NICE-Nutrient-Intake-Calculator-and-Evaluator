def calculatorMale():
    weight = float(input("Please enter your weight in kilograms: "))
    height = float(input("Please enter your height in centimeters: "))
    age = int(input("Please enter your age in years: "))
    lifestyle =input("Do you work a physical job or have an active lifestyle? (yes/no): ").strip().lower()
    bmr = 66.5 + (13.75 * weight) + (5 * height) - (6.76 * age)
    if lifestyle == "yes":
        bmr *= 1.75  
    print(f"Your BMR is: {bmr}")

def calculatorFemale():
    weight = float(input("Please enter your weight in kilograms: "))
    height = float(input("Please enter your height in meters: "))
    age = int(input("Please enter your age in years: "))
    lifestyle = input("Do you work a physical job or have an active lifestyle? (yes/no): ").strip().lower()
    bmr = 655 + (9.56 * weight) + (1.85 * height) - (4.67 * age)
    if lifestyle == "yes":
        bmr *= 1.75  
    print(f"Your BMR is: {bmr}")

def calc():
    choice = int(input("Please select your gender (1 Male / 2 Female): "))
    if choice == 1:
        calculatorMale()
    elif choice == 2:
        calculatorFemale()
    else: print("We are Charlie Kirk") 
    