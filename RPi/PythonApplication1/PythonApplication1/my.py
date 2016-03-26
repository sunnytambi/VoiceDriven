import subprocess
import json
import xml.dom.minidom
import webbrowser
from managers import YoutubeManager, GeneralManager

# Initializations
#API_KEY = "AIzaSyDGdH6lhqaZ0ohKj95czvAZ5nK_4pJEqkw"
#GOOGLE_SPEECH_URL_V2 = "https://www.google.com/speech-api/v2/recognize?output=json&lang=en-us&key=%s&client=chromium&maxresults=6&pfilter=2" % (API_KEY);

DOMTree = xml.dom.minidom.parse('Commands.xml')
command_nodes = DOMTree.documentElement.getElementsByTagName("command")
gm = GeneralManager.GeneralManager(command_nodes)
gm.manage("Youtube why does sun rotate", None)

output = subprocess.check_output("./shell.sh", shell=True).decode('utf-8')
print('python='+output)

output = output.replace('{"result":[]}', '')
jsonOut = json.loads(output)
current_program = None

for result in jsonOut['result']:
  if result['final'] == True:
#  #  for alternative in result['alternative']:
#  #    for transcript in alternative
         spoken_sentence = result['alternative'][0]['transcript']
         gm.manage(spoken_sentence, None) # initialization/first time call. Rest calls will be taken care by General Manager



         #spoken_split = spoken_sentence.split(' ', 1)
         #if(len(spoken_split) >= 2):
         #  spoken_command = spoken_split[0].lower()
         #  command_args = spoken_split[1].lower()
         #  DOMTree = xml.dom.minidom.parse('Commands.xml')
         #  commands = DOMTree.documentElement.getElementsByTagName("command")
         #  for command in commands:
         #    if(command.getElementsByTagName("key")[0].childNodes[0].data == spoken_command):
         #      print("Processing your command = "+command.getElementsByTagName("program")[0].childNodes[0].data.format(command_args))
         #      webbrowser.open(command.getElementsByTagName('program')[0].childNodes[0].data.format(command_args.replace(' ', '%20')))
         #elif(len(spoken_split) == 1):
         #  internal_command = spoken_split[0].lower()
         #  if(internal_command == 'quit'):
         #    subprocess.call('pkill '+epiphany)
