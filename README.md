# Change Tracker

Used Nugets:
1. Newtonsoft JSON (230M downloads) - parsing JSON files and serialization
2. DeepEqual (291K downloads) - Fair deep equality for objects

Main point for this change tracker is build easy and configurable pipeline for your model.
For this I used extension methods and Builder pattern.
This realization also have generator which can generate complex and full tree of models with random and pretty good names.

Using:
Just compile and run .exe. In folder of .exe will be generated 3 files: 'original.json', 'updated.json', 'result.json'.
You can change first and second. Third is audit logs file based on diff. After changing of first or second file just restart app and will be generated new 'result.json'. 'original.json' and 'updated.json' generating only if in directory of .exe they are missed. 


FAQ:

Q1: Why objects haven't Id field?

A1: Creating separate sections for types will be not fully show all tree of instances and dependencies. If we show all dependencies without ID, ID in this case is just unnessesary field.

Q2: Why we can't create new Station on route?

A2: Because making pairs in this case will be more hard.

Q3: Can we use ID field for this?

A3: Yes, I got this when writing this text :) (Can be changed if it will be needed)

P.S.: About start date, end date and affected days of log element. Used logic: after grouping we take first and last updated elements dates. Affected days is set of ALL days that not dependencies how much day was in enumeration.

If you will have any additional question about this, just chat to me :)
