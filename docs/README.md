# NICE-Nutrient-Intake-Calculator-and-Evaluator-

[IN DEVELOPEMENT] NICE is designed to help people understand what their body craves and aide in eating healthy using the calendar feature to track their food.

## Core features

- Calculator: Enter your body weight, height and age and get a recommendation on nutrient intake structured according to your needs
- Calendar: Add foods to your personal shedule and see your weekly nutrient coverage
- [Future feature] Personal AI assistance structured to assist you by analyzing your calendar

## Requirements

- Python

## Installation

For convinience purposes i recommend using Python Installer to create a executeable exe file and storing it on your desktop. Copy and paste the following code inside the terminal
<code>
git clone https://github.com/MaximKohanov/NICE-Nutrient-Intake-Calculator-and-Evaluator-.git
cd NICE-Nutrient-Intake-Calculator-and-Evaluator
python -m venv venv
venv\Scripts\activate
pip install -r requirements.txt
python -m pyinstaller main.py --onefile --icon NICE.ico
<code>
