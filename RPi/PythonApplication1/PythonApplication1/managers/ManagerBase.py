class ManagerBase():
    """Base/Abstract class for all managers"""

    def __init__(self, command_node):
        self.program = None
        pass

    def manage(self, command_str, args):
        pass

    def getManagedCommand(self):
        pass

    def destroy(self):
        pass