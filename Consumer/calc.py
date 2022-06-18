# coding: utf-8
from datetime import datetime
l1=[]

with open("dump") as f:
    for line in f:
        l1.append(line)


soj_waits=[]
waits=[]
for l in l1:
    ss=l.split(' ')
    t1=datetime.strptime(ss[0],'%H:%M:%S')
    t3=ss[2].replace('\n','')
    t3=datetime.strptime(t3,'%H:%M:%S')
    t2=datetime.strptime(ss[1],'%H:%M:%S')
    diff=t3-t1
    diff2=t2-t1
    seconds=diff.total_seconds()
    seconds2=diff2.total_seconds()
    waits.append(seconds2)
    soj_waits.append(seconds)
    
    

avg=sum(waits)/len(waits)
avg2=sum(soj_waits)/len(waits)
print(f'length is {len(waits)}')
print()
print("waiting    ",avg)
print("soj waiting",avg2)
