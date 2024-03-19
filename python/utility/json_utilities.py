import json

class JSONUtilities:
    def __init__(self, json_text):
        self.json_text = json_text

    def is_valid_json(json_text):    
        return (json_text.startswith("{") and json_text.endswith("}")) or (json_text.startswith("[") and json_text.endswith("]"))
        