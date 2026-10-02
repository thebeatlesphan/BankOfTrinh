# bankoftrinh.client

The frontend application for BankOfTrinh, a modern banking simulator built with Angular for learning and portfolio practice.

The frontend will provide a polished user interface for interacting with the BankOfTrinh backend API. It will be used to practice Angular application structure, components, services, routing, reactive forms, validation, HTTP communication, state management, responsive design, accessibility, and frontend testing.

## Current Status

The backend currently provides functionality for:

- Customer management
- Bank account management
- Deposits
- Account balance updates
- Persisted account transactions
- Request validation
- Integration testing

The frontend will be developed incrementally by completing one full user journey at a time.

## Visual Direction

BankOfTrinh should feel like a calm, thoughtful financial workspace rather
than a traditional banking portal or generic admin dashboard.

The visual language uses warm neutrals, charcoal typography, muted sage accents,
and restrained coral highlights. The interface should feel trustworthy and
structured while remaining approachable and distinctive.

### Design Personality

The interface should be:

- Grounded rather than flashy
- Human rather than corporate
- Clear rather than dense
- Warm rather than cold
- Confident rather than aggressive

Avoid:

- Heavy navy application shells
- Generic Bootstrap dashboard styling
- Excessive gradients
- Large decorative illustrations
- Dense data tables
- Overuse of accent colors
- Unnecessary animations

## Project Structure

```text
bankoftrinh.client/
├── src/
│   ├── app/
│   │   ├── app.routes.ts
│   │   ├── app.config.ts
│   │   └── app.component.html
│   │   └── app.component.css
│   │   └── app.component.ts
│   │   └── app.component.spec.ts
│   │   └── app.module.ts
│   │
│   ├── assets/
│   ├── environments/
│   ├── index.html
│   ├── main.ts
│   └── styles.css
│
├── angular.json
├── aspnetcore-https.js
├── karma.conf.js
├── package.json
├── tsconfig.json
└── README.md

```
