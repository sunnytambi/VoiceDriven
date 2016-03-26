import sys
from apiclient.discovery import build
from apiclient.errors import HttpError
from oauth2client.tools import argparser
from subprocess import Popen, PIPE, call, check_output, STDOUT
import codecs
from PIL import Image
from termcolor import colored
from platform import system
from isodate import parse_duration
import colorama

class YoutubeRenderer(object):
    """Displays Youtube video in native Youtube client"""

    __API_KEY = "AIzaSyDGdH6lhqaZ0ohKj95czvAZ5nK_4pJEqkw"
    __YOUTUBE_API_SERVICE_NAME = "youtube"
    __YOUTUBE_API_VERSION = "v3"
    __myvideos = []
    youtube = None
    displayVideos = None

    def render(self, search_args):
        argparser.add_argument("--q", help="Search term", default=search_args)
        argparser.add_argument("--max-results", help="Max results", default=25)
        argparser.add_argument("--type", help="Type of result", default="video")
        args = argparser.parse_args()
        try:
            self.__youtube_search(args)
        except HttpError as e:
            print ("An HTTP error %d occurred:\n%s" % (e.resp.status, e.content))


    def __youtube_search(self, options):
      self.youtube = build(self.__YOUTUBE_API_SERVICE_NAME, self.__YOUTUBE_API_VERSION,
        developerKey=self.__API_KEY)

      # Call the search.list method to retrieve results matching the specified
      # query term.
      search_response = self.youtube.search().list(
        q=options.q,
        part="id",
        maxResults=options.max_results
      ).execute()

      self.__handleSearch__(search_response)
      self.__askUserToPlay()
    
    def __askUserToPlay(self):
        
      self.__printResults__(self.displayVideos)
      print(colorama.Fore.WHITE + "")
      user_in = input("Enter the number of the video to search: ")
      user_in = int(user_in) - 1
      player = "mpsyt"

      #if(system() == "Windows"):
      #    player =  "mpv"
      print("Playing... \""+self.__myvideos[user_in]['Title']+"\" ... on " + player)
      #Popen(["start", "mpsyt", "set show_video 1, set player mpv, playurl " + self.__myvideos[user_in]["id"]["videoId"]], 
      #      shell=True, stdout=PIPE, stderr=STDOUT)
      ps = None
      if(system() == "Windows"):
          ps = Popen(["start", "/wait", player, "playurl", "https://www.youtube.com/watch?v="+self.__myvideos[user_in]["Id"]], 
                shell=True, stdout=PIPE, stderr=STDOUT)
      else:
          ps = Popen([player, "playurl", "https://www.youtube.com/watch?v="+self.__myvideos[user_in]["Id"]], 
                stdout=PIPE, stderr=STDOUT)
      ps.communicate()
      self.__askUserToPlay()

    def __handleSearch__(self, search_response):
        
      self.displayVideos = []
      channels = []
      playlists = []
      counter = 0
      videoidx = []

      # Add each result to the appropriate list, and then display the lists of
      # matching videos, channels, and playlists.
      for search_result in search_response.get("items", []):
        if(search_result["id"]["kind"] == "youtube#video"):
            videoidx.append(search_result["id"]["videoId"])
                #search_result["snippet"]["thumbnails"]["medium"]["url"]))

        #elif search_result["id"]["kind"] == "youtube#channel":
        #  channels.append("%s (%s)" % (search_result["snippet"]["title"],
        #                               search_result["id"]["channelId"]))
        #elif search_result["id"]["kind"] == "youtube#playlist":
        #  playlists.append("%s (%s)" % (search_result["snippet"]["title"],
        #                                search_result["id"]["playlistId"]))
      
      duration_response = self.youtube.videos().list(
        part="id,snippet,contentDetails",
        id=",".join(videoidx)
      ).execute()

      for duration_result in duration_response.get("items", []):
          title = self.removeNonAscii(duration_result['snippet']['title'])
          dur = parse_duration(duration_result['contentDetails']['duration']).total_seconds()
          m, s = divmod(dur, 60)
          h, m = divmod(m, 60)
          id = duration_result["id"]
          dict = {'Id': id, 'Title': title};
          self.__myvideos.append(dict)
          counter += 1
          self.displayVideos.append("%3d.  %60s (%11s) \t%d:%02d:%02d" % (
              counter,
              title[:60].ljust(60, ' '),#.encode(encoding='UTF-8',errors='replace'),
              id,
              h, m, s
          ))

      #print u"Stocker".encode(sys.stdout.encoding, errors='replace')
      #print("playing .. "+myvideos[0]["id"]["videoId"])
      #subprocess.call("vlc https://www.youtube.com/watch?v="+myvideos[0]["id"]["videoId"], shell=True )
      #print ("Channels:\n")
      #print(channels) 
      #print("\n")
      #print ("Playlists:\n")
      #print(playlists)
      #print("\n")

    def __printResults__(self, videos):
        print(colorama.Fore.WHITE + "{}".format("\t\t\t\t=========VIDEOS========"))
        print(colorama.Style.BRIGHT)
        fColor = colorama.Fore.YELLOW
        
        for vid in videos:
        
            if(fColor == colorama.Fore.GREEN):
                fColor = colorama.Fore.YELLOW
            else:
                fColor = colorama.Fore.GREEN
            #cv2.namedWindow('image', cv2.WINDOW_NORMAL)
            #cv2.imshow('image',img)
            #image = Image.open(vid["snippet"]["thumbnails"]["medium"]["url"])
            #image.show()
            #print( "Content-type: image/png\n")
            #print( file(r""+vid["snippet"]["thumbnails"]["medium"]["url"], "rb").read())
            print(fColor + "{}".format(vid))
            #print("\n")
        print(colorama.Style.NORMAL)
        #colorama.deinit()

    def removeNonAscii(self,s): 
        return "".join(i for i in s if ord(i)<128)