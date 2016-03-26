import time
from threading import Thread

class worker(Thread):
    """description of class"""
    def run(self):
        for x in xrange(0,11):
            print(x)
            time.sleep(1)

class waiter(Thread):
    """description of class"""
    def run(self):
        for x in xrange(100,103):
            print(x)
            time.sleep(5)

def run():
    worker().start()
    waiter().start()