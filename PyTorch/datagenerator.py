import csv
import random


class Characteristic:
    def __init__(self, label, feedbackboundaries, sensoryboundaries, readingboundaries, structureboundaries):
        self.label = label
        self.feedbackboundaries = feedbackboundaries
        self.sensoryboundaries = sensoryboundaries
        self.readingboundaries = readingboundaries
        self.structureboundaries = structureboundaries

    def getentry(self):
        return [random.uniform(self.feedbackboundaries[0], self.feedbackboundaries[1]),
                random.uniform(self.sensoryboundaries[0], self.sensoryboundaries[1]),
                random.uniform(self.readingboundaries[0], self.readingboundaries[1]),
                random.uniform(self.structureboundaries[0], self.structureboundaries[1]),
                self.label]


# set the filename and number of rows to generate
filename = 'asd-data.csv'
num_rows = 10000

# define what characteristicts to be generated rows from
characteristics = [Characteristic("pictogram", [0, 5], [0, 5], [0, 2], [3, 5]),
                   Characteristic("text", [0, 5], [0, 2], [3, 5], [3, 5]),
                   Characteristic("pictogramandtext", [0, 5], [0, 1], [3, 5], [4, 5]),
                   Characteristic("static", [4, 5], [0, 1], [0, 5], [0, 5])]

# create a list to hold the rows
rows = []

# generate random rows and append them to the list
for i in range(num_rows):
    rows.append(random.choice(characteristics).getentry())

# write the rows to a new CSV file
with open(filename, 'w', newline='') as csvfile:
    writer = csv.writer(csvfile)
    for row in rows:
        writer.writerow(row)

print("Generated ", num_rows, " entries.")
