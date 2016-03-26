from subprocess import Popen, PIPE, call, check_output, STDOUT
import webbrowser
import xml.dom.minidom
import sys
from threading import Thread, Timer
#import numpy as np
#import cv2
from queue import Queue, Empty
from managers import ManagerBase
from renderers import YoutubeRenderer
#from managers.YoutubeManager import numpy, cv2

class YoutubeManager(ManagerBase.ManagerBase):
    """Manages Youtube search and navigation"""

    __browserProc__ = None

    def __init__(self, command_node):
        self.program = command_node.getElementsByTagName("program")[0].childNodes[0].data
    

    io_q = Queue()
    p1 = None
    def stream_watcher(self,identifier, stream):

        for line in stream:
            if(line.decode('utf-8') != "\r\n"):
                self.io_q.put(identifier, line)

        if not stream.closed:
            stream.close()

    def manage(self, command_str, args):
        if(command_str != None):
            if(command_str == "close"):
                self.destroy()
                del self
            elif(command_str == "first"):
                print("selecting first result in youtube")
            elif(args != None):
                #print("i am opening the mps-youtube thing")
                #self.__browserProc__ = subprocess.Popen("mpsyt search " + args, shell=True)
                #call("mpsyt set show_video 1, set player mpv, search " + args, shell=True)
                #self.p1 = Popen(["start","mpsyt", "set show_video 1, set player mpv, search " + args], 
                #                      shell=True, stdout=PIPE, stderr=STDOUT)
                #output = check_output("start mpsyt set show_video 1, set player mpv, search " + args, 
                #             shell=True, stderr=STDOUT, timeout=10).decode('utf-8')
                #self.p1.communicate("1")
                
                #tim = Timer(10.0, self.hello)

                #t1 = Thread(target=self.stream_watcher, name='stdout-watcher',
                #            args=('STDOUT', self.p1.stdout))
                #tim.start()
                #t1.join(timeout=10)
                #sys.stdout.write("1 \n\r")
                #self.p1.communicate("1")
                #print("i m back with ")
                #Thread(target=self.printer, name='printer').start()
                #sys.stdout.write("mpsyt set show_video 1, set player mpv, search " + args + " \n\r")
                #sys.stdout.flush()
                #webbrowser.open(self.program.format(args.replace(' ', '%20')))
                #cap = cv2.VideoCapture('http://www.sample-videos.com/video/mp4/720/big_buck_bunny_720p_1mb.mp4')
                #while(cap.isOpened()):
                #    ret, frame = cap.read()

                #    if(frame != None):
                #        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
                #        cv2.imshow('frame',gray)
                    
                #    if cv2.waitKey(1) & 0xFF == ord('q'):
                #        break

                #cap.release()
                #cv2.destroyAllWindows()

                renderer = YoutubeRenderer.YoutubeRenderer()
                renderer.render(args)
                #print("Now you can navigate through Youtube by saying 'First', 'Last', etc.")

    def hello(self):
        print("hello, world")
        self.p1.communicate("1")

    def printer(self):
        while True:
            try:
                # Block for 1 second.
                item = self.io_q.get(True, 1)
            except Empty:
                # No output in either streams for a second. Are we done?
                try:
                    if self.p1.poll() is not None:
                        break
                    else:
                        identifier, line = item
                        print(identifier + ':', line)
                except:
                    # do nothing
                    self=self

    def getManagedCommand(self):
        return "youtube"

    def destroy(self):
        print("destroying youtube manager..")
        self.program = None
        # Kill the browser window
        subprocess.call("pkill epiphany")
