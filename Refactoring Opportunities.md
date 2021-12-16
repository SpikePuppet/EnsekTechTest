# Refactoring Opportunities
## Overview
This document serves as a reflection on what i have built for this technical test, as well as a critque of what i built and how i would improve it in future iterations. This is not final, as there is always something to be improved in any project. 

## Notes
Currently, we use a very simple implementation of the Repository Pattern (as seen in the service EntityService and it's interface IEntityService<T>). This works for this simple example, and has been very useful. It has allowed me to build generic services quickly, however it's simplicity is obvious. If i was to improve this in future, i would spend some more time making sure these services were more robust and better at error handling. Alternatively, i would also use an existing project i contribute too called Atlas (https://github.com/SpikePuppet/atlas) which handles this.

To extend the above point, i would also make the generic controller i created a lot more robust, though for this example I'm happy with how it's laid out. Both generic services enabled quick iteration of code.

The CsvParsingService relies on the package CsvHelper. This service is particularly nice, as any records in the file which don't match the class map are discarded so we don't need to handle any particuarly deep validation in that area. 
