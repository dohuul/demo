class Tree(object):
    name = 'Tree'
    def print(self):
        return self.name

class Banyan(object):
    def name(self):
        return 'Banyan'

    def print(self):
        return self.name()

class Peepal(object):
    def name(self):
        return None

    def print(self):
        return 'Peepal'