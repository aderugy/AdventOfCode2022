import re

def calc(amin, amax, bmin, bmax):
    amin = int(amin)
    bmin = int(bmin)
    amax = int(amax)
    bmax = int(bmax)
    return not(amin > bmax or bmin > amax)

def split_string(s):
    return re.split('-|,', s) 


with open("input") as f:
    lines = f.readlines()
    sum = 0
    for line in lines:
        line = line[:len(line) - 1]
        line_split = split_string(line)
       
        print(line_split)
        if len(line_split) != 4:
            continue

        if calc(line_split[0], line_split[1], line_split[2], line_split[3]):
            sum += 1
        
    print(sum)
while True:
    s = input(":")
    line_split = split_string(s)
    print(line_split, calc(line_split[0], line_split[1], line_split[2], line_split[3]))
