import datetime
import os
import src.NICE.configuration as configuration
from pathlib import Path


def setupNICEFolder():
    Path(Path.home()/"NICE").mkdir(parents=True, exist_ok=True)
    with open(configuration.NICElog, "w+") as f:
        f.write(f"[{datetime.datetime.now()}] NICE logging initiated\n")
    
def logAction(message):
    with open(configuration.NICElog, "a") as f:
        f.write(f"[{datetime.datetime.now()}] {message}\n")

def isFirstStart():
   if not configuration.NICEFolderPath.exists():
       return True