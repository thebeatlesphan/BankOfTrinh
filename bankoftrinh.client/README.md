# bankoftrinh.client

The frontend application for BankOfTrinh, a banking simulator built with Angular for learning and portfolio practice.

The frontend will provide a user interface for interacting with the BankOfTrinh backend API. It will be used to practice Angular application structure, components, services, routing, forms, validation, HTTP communication, state management, and frontend testing.

## Current Status

The Angular frontend has not been implemented yet.

The backend currently provides functionality for:

- Customer management
- Bank account management
- Deposits
- Account balance updates
- Persisted account transactions
- Request validation
- Integration testing

The frontend will be developed incrementally by connecting user-facing screens to the existing backend API.

## Planned Features

### Customer Management

- Create customer form
- Customer details view
- Customer lookup
- Customer validation messages
- Duplicate email error handling
- Missing customer error handling

### Bank Account Management

- Create bank account for an existing customer
- View individual bank account
- View all accounts belonging to a customer
- Display account number
- Display current balance
- Display account owner
- Missing account error handling

### Account Transactions

- Deposit form
- Withdrawal form
- Transaction history
- Transaction type display
- Transaction amount display
- Balance-after-transaction display
- Transaction timestamp display
- Validation for invalid transaction amounts
- Insufficient balance error handling

### User Interface

- Navigation between application views
- Reusable form components
- Reusable error-message components
- Loading indicators
- Empty-state messages
- Success messages
- API error messages
- Responsive layout
- Basic accessibility support

### Testing

- Component tests
- Service tests
- Form validation tests
- HTTP client tests
- Routing tests
- Error-state tests
- Loading-state tests
- End-to-end tests for key user journeys

## Initial User Journeys

The frontend will initially support the following workflow:

```text
Create customer
    ↓
Create bank account
    ↓
View account details
    ↓
Make deposit
    ↓
View updated balance
    ↓
View transaction history


## Project Structure

```text
bankoftrinh.client/
├── src/
│   ├── app/
│   │   ├── core/
│   │   │   ├── errors/
│   │   │   ├── interceptors/
│   │   │   ├── models/
│   │   │   └── services/
│   │   │
│   │   ├── shared/
│   │   │   ├── components/
│   │   │   ├── directives/
│   │   │   └── pipes/
│   │   │
│   │   ├── features/
│   │   │   ├── customers/
│   │   │   │   ├── create-customer/
│   │   │   │   ├── get-customer/
│   │   │   │   └── customer.models.ts
│   │   │   │
│   │   │   ├── accounts/
│   │   │   │   ├── create-account/
│   │   │   │   ├── account-details/
│   │   │   │   ├── customer-accounts/
│   │   │   │   └── account.models.ts
│   │   │   │
│   │   │   └── transactions/
│   │   │       ├── deposit/
│   │   │       ├── withdrawal/
│   │   │       ├── transaction-history/
│   │   │       └── transaction.models.ts
│   │   │
│   │   ├── app.routes.ts
│   │   ├── app.config.ts
│   │   └── app.component.ts
│   │
│   ├── assets/
│   ├── environments/
│   ├── index.html
│   ├── main.ts
│   └── styles.css
│
├── angular.json
├── package.json
├── tsconfig.json
└── README.md
