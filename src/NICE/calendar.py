# required imports
import json
import features
import configuration
import storage

#custom imports

with open("data/calendar.json", "r") as calendar_file:
    calendar_data = json.load(calendar_file)

with open("data/foods.json", "r") as foods_file:
    foods_data = json.load(foods_file)

def calendar():
    print("Calendar feature is being actively developed.")

