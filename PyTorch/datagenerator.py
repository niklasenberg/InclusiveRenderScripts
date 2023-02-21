import csv
import random

class Characteristic:
    def __init__(self, label, motoricsBoundaries, sensoryBoundaries, readingBoundaries, structureBoundaries):
        self.label = label
        self.motoricsBoundaries = motoricsBoundaries
        self.sensoryBoundaries = sensoryBoundaries
        self.readingBoundaries = readingBoundaries
        self.structureBoundaries = structureBoundaries

    def getEntry(self):
        return [random.uniform(self.motoricsBoundaries[0], self.motoricsBoundaries[1]),
                random.uniform(self.sensoryBoundaries[0], self.sensoryBoundaries[1]),
                random.uniform(self.readingBoundaries[0], self.readingBoundaries[1]),
                random.uniform(self.structureBoundaries[0], self.structureBoundaries[1]),
                self.label]


# set the filename and number of rows to generate 5.1,3.5,1.4,0.2,pictogram
filename = 'asd-data.csv'
num_rows = 1500
characteristics = [Characteristic("pictogram", [0, 5], [0, 5], [0, 2], [3, 5]),
                   Characteristic("text", [0, 5], [3, 5], [3, 5], [3, 5]),
                   Characteristic("pictogramandtext", [0, 5], [4, 5], [3, 5], [4, 5])]

# create a list to hold the rows
rows = []

# generate random rows and append them to the list
for i in range(num_rows):
    rows.append(random.choice(characteristics).getEntry())

# write the rows to a new CSV file
with open(filename, 'w', newline='') as csvfile:
    writer = csv.writer(csvfile)
    for row in rows:
        writer.writerow(row)