# spur-store-finance-manager
TASK-BASED SOFTWARE ENGINEERING : COSE50584-2014-SUG2-2014-SUG2

Spur Ltd are a company that consists of 100 convenience stores in various locations around the
UK. The have a modern Point of Sale system however their Supply Ordering system is a little out
dated and they feel parts of their ordering process can be improved. Currently stores make orders
with their suppliers independently of each other, this causes an issue for the finances team at Spur
Headquarters as they find it very hard to collect all the information for the supplier orders made
from the independent stores.

Each independent store generates numerous .csv files (1 a week) that contain the data for all the
orders they make. The finances team would like a fast and responsive system that will allow them
to analyse the data sent from the stores. The Spur Technical Team has an investment into
Microsoft technologies and as such would like this system developed using C#.Net. They also feel
the use of the Task Parallel Library will allow faster response times in the application and wish for
the application to make use of appropriate TPL features.

The finance team already have a method of receiving the files from the stores, they get placed into
a central folder which they hope this new application will be able to read from. You will be supplied
with 2 years of historical order data for each of the 100 stores to help you test your solution.

##Requirements
- An easy to use command line interface.
- All features should be calculated when the option is selected in the UI, not at initial load.
This will allow the data sets to be updated and the calculations rerun without reopening
the application. Spur's technical team would like the UI to continue being responsive
during these calculations.
- Have the flexibility to point the application to a certain folder on a PC to find the .csv files.
- List all stores, suppliers and supplier types.
- Allow the Finances team to find the following data:-
  - The total cost of all orders available in the supplied data
  - The total cost of all orders for a single store
  - The total cost of orders in a week for all stores
  - The total cost of orders in a week for a single store
  - The total cost of all orders to a supplier
  - The cost of all orders from a supplier type
  - The cost of orders in a week for a supplier type
  - The cost of orders for a supplier type for a store
  - The cost of orders in a week for a supplier type for a store
Advanced Requirements
- A Graphical User Interface using WinForms or WPF
- The ability to plot the historical supplier order data on a graph
  - Spur do not mind if a 3rd Party Graph control is used, some suggestions are below
- Other features the developer feels suitable may also be rewarded
