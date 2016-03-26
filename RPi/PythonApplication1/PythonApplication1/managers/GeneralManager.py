from managers import ManagerBase
from managers import YoutubeManager
import colorama
from subprocess import call
from platform import system

class GeneralManager(ManagerBase.ManagerBase):
    """Handles all the commands if there is no specific manager available"""

    current_manager = None
    manager_list = {}

    def __init__(self, command_nodes):
        self.command_nodes = command_nodes
        #  arecord --duration=5 --channels=1 --format=cd --rate=16000 -t wav -D plughw:1,0 recording.wav
        for command in command_nodes:
            self.manager_list[command.getElementsByTagName("key")[0].childNodes[0].data] = command

    def manage(self, spoken_sentence, args):
        if(system() == "Windows"):
            call("cls", shell=True)
        else:
            call("clear")
        spoken_command = None
        command_args = None
        printable_sentence = "Spoken Sentence:-"

        #print(+spoken_sentence)
        spoken_split = spoken_sentence.split(" ", 1)
        if(len(spoken_split) >= 1):
            spoken_command = spoken_split[0].lower()
            printable_sentence += " " + colorama.Fore.GREEN + "{}".format(spoken_command)
            #print("Command:- "+spoken_command)
        if(len(spoken_split) >= 2):
            command_args = spoken_split[1].lower()
            printable_sentence += " " + colorama.Fore.CYAN + "{}".format(command_args)
            #print("Command Args:- "+command_args)

        print(printable_sentence)			
        
        self.current_manager = self.getManager(spoken_command)
        if(self.current_manager != None):
            self.current_manager.manage(spoken_command, command_args)

    def getManagedCommand(self):
        super().getManagedCommand()

    def getManager(self, command_str):
        mgr = None;
        if(command_str != None):
            if(self.current_manager != None):
                # If current manager is youtube and user asks to 'close' it
                if(command_str not in self.manager_list):
                    mgr = self.current_manager
                    return mgr
                else:
                    self.current_manager.destroy()
                    del self.current_manager
                    self.current_manager = None

            if(command_str == 'youtube'):
                mgr = YoutubeManager.YoutubeManager(self.manager_list[command_str])
            elif(command_str == 'google'):
                mgr = None

        return mgr

    def destroy(self):
        return super().destroy()