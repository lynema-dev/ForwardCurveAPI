import os
import clr
import ctypes as ct
import pandas as pd
import numpy as np
import matplotlib.pylab as plt
from pandas.plotting import register_matplotlib_converters

def main():
    
    #Looks for dll in the same directory as this file
    curdirectory = str(os.path.dirname(os.path.realpath(__file__))) + '\\'
    clr.AddReference(curdirectory + 'c_sharp_dll.dll')
    from c_sharp_dll import DynamicCalc
    target = DynamicCalc()
    assert target.DynamicCalcEcho() == "echo"

    #Forward Curve example
    tenors = (ct.c_double*10) (*range(10))
    tenors[0] = 1
    tenors[1] = 3
    tenors[2] = 5
    tenors[3] = 10
    tenors[4] = 15
    tenors[5] = 20
    tenors[6] = 25
    tenors[7] = 30
    tenors[8] = 40
    tenors[9] = 50

    rates = (ct.c_double*10) (*range(10))
    rates[0] = 0.025
    rates[1] = 0.03
    rates[2] = 0.032
    rates[3] = 0.035
    rates[4] = 0.0355
    rates[5] = 0.0345
    rates[6] = 0.0335
    rates[7] = 0.033
    rates[8] = 0.0325
    rates[9] = 0.032

    valuationdate = 20200630
    frequency = 6

    #call to API
    curves = target.ForwardCurveCalc(tenors, rates, valuationdate, frequency)

    #convert results to dataframe
    arr = SystemDoubleArrayToNumpyArray(curves)
    columns = ['time','date','spotrate','forwardrate']
    df = pd.DataFrame(data=arr, columns=columns)
    df.drop(df.index[:1], inplace=True)

    #plot results
    register_matplotlib_converters()
    plt.figure(figsize=(10,6)) 
    plt.plot(df['time'], df['forwardrate'], color = 'slategrey', label = 'forward')
    plt.plot(df['time'], df['spotrate'], color = 'black', label = 'spot')
    title = 'Spot and Forward Rates, frequency ' + str(frequency) + 'm, date ' + str(valuationdate)
    plt.title(title)
    plt.legend(loc="upper left")
    plt.show()

def SystemDoubleArrayToNumpyArray(Arr):
    hBound = Arr.GetUpperBound(0) + 1
    vBound = Arr.GetUpperBound(1) + 1
    resultArray = np.zeros([hBound, vBound])

    for c in range(hBound):            
            for r in range(vBound):
                resultArray[c,r] = Arr[c,r]
    return resultArray


if __name__ == '__main__':
    main()








