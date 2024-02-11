# Challenge-Application

# The task
1. The task is to implement a money withdrawal in the WithdrawMoney.Execute(...) method in the features folder. For consistency, the logic should be the same as the TransferMoney.Execute(...) method i.e. notifications for low funds and exceptions where the operation is not possible.

2. As part of this process however, you should look to refactor some of the code in the TransferMoney.Execute(...) method into the domain models, and make these models to be less possible for misuse. We're looking to make our domain models rich in behaviour and much more than just plain old objects, however we don't want any data persistance operations in our domain model (i.e. data access repositories). This should simplify the task 1. of implementing WithdrawMoney.Execute(...).

# Result
1. Add WithdrawMoney.Execute(...) inplementation.
2. Add some data validation and possible issues check.
3. Add xUnit tests for WithdrawMoney.Execute(...) and TransferMoney.Execute(...) methods.
4. Used Mock library for IAccountRepository and INotificationService in tests.
5. Add "// TODO: Transaction required" for TransferMoney.Execute(...).
6. Updated project version to .Net core 3.1 because of old version vulnerability.
