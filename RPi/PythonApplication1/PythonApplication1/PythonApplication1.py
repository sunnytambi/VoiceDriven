import subprocess
import json
import xml.dom.minidom
import webbrowser
import sys
from managers import YoutubeManager, GeneralManager
#from AudioRecorder import TapTester
import colorama

# https://progfruits.wordpress.com/2014/05/31/using-google-speech-api-from-python/

colorama.init()
DOMTree = xml.dom.minidom.parse('Commands.xml')
command_nodes = DOMTree.documentElement.getElementsByTagName("command")

gm = GeneralManager.GeneralManager(command_nodes)
user_search = input('Search Youtube for: ')
gm.manage("Youtube "+user_search, None) # initialization/first time call. Rest calls will be taken care by General Manager
#another_command = input()
#print('got another command..  '+another_command)
#gm.manage(another_command, None)
#gm.manage("close", None)

#tstr = TapTester()
#while(True):
    #tstr.listen()

# subprocess.call(["dir"], shell=True)
# output = subprocess.check_output(["dir"], shell=True).decode('utf-8')
#output = '{"result":[{"alternative":[{"transcript":"good morning Google how are you feeling today","confidence":0.97335243}],"final":true}],"result_index":0}'
#jsonOut = json.loads(output)

#for result in jsonOut['result']:
#    if result['final'] == True:
#        for alternative in result['alternative']:
#            print(alternative['transcript'])
# print(jsonOut['result'][0]['alternative'][0]['transcript'])
#print(jsonOut['result'][0]['final'])