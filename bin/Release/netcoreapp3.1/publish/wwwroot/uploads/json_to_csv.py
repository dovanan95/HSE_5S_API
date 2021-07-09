import pandas as pd
from pandas.io.json import json_normalize
import json
import numpy
import flatten_json
from flatten_json import flatten


def js_r(filename: str):
    with open(filename) as f_in:
        return json.load(f_in)

data = js_r('D:\\Indoor Positioning System\\dev\\data\\idls-db-default-rtdb-export_5.json')

#print(dick_flat)
#print(data['wifi_measurement']['-MYd2aYLGvYHwRAxDDjq'])
flatten_csv = pd.DataFrame()
for x in data['data']:
    e = data['data'][x]
    f= flatten(e)
    flatten_csv = flatten_csv.append(f, ignore_index=True)

print(len(flatten_csv))
#flatten_csv.to_csv('result.csv',sep=',')

    

