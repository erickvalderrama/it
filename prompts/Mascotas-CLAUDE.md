# CLAUDE.md — Mascotas Project

## Project and stack

Build the requested change in small, verifiable increments. Mascotas connects people with adoption foundations, independent pet service providers, and pet supply stores.
Use C# with ASP.NET Core Web API, PostgreSQL, Entity Framework Core, and Npgsql.
Use one Flutter and Dart application for web, Android, and iOS. Use Riverpod as the initial state management choice and respect a state solution already approved for the project.
Check existing dependency versions and official compatibility guidance before introducing or upgrading packages.

## Golden rule — Approved entity relationship model

The approved entity relationship model governs persistence. Record its path here when available. Approved model path is pending.
- Match entities, columns, types, precision, nullability, keys, cardinalities, constraints, and deletion rules.
- Derive mappings and migrations from the approved model. Treat the functional modules and role names below as requirements for behavior.
- Use English code identifiers and explicit mappings for physical database names required by the model. Resolve schema naming conflicts with the approved model before generating a migration.
- When the approved model is unavailable for a persistence change, identify the missing input and continue work independent of the schema.
- For a feature that requires an unapproved table, field, relationship, enum value, or change to a constraint, document the exact model change and its impact. Continue independent work and apply a coordinated model, mapping, migration, and test change once authorized.
- Reconcile any discrepancy between the model, migrations, and existing database before altering persistence. Apply real database migrations or destructive operations only within the user's authorized scope.

## English throughout the codebase

Use English for all authored code, filenames, directories, namespaces, classes, methods, variables, constants, comments, API routes, DTOs, error codes, fixtures, and test names. Use English names in new database models and migration code while preserving approved physical mappings.
Use English for base interface copy. Keep any future translations in dedicated localization resources, separate from source code. Preserve user-entered content and the product name Mascotas.
Store all base UI copy in named English constants. Keep common copy in a shared catalog such as AppStrings and feature-specific copy in that feature's catalog. Render labels through references such as Text(AppStrings.checkoutTitle). Include titles, labels, hints, actions, dialogs, validation messages, and loading, empty, and error states.
Use templates with parameters for changing values such as prices, dates, and counts. Render business data supplied by the API and map stable API error codes to centralized user messages. Keep environment URLs, secrets, rates, and plan values in their proper configuration or data source.

## Clean architecture and file limit

Organize the backend as a modular monolith.
- Domain owns entities, value objects, and invariants. Keep it independent of persistence, HTTP, and payment providers.
- Application owns use cases, DTOs, validation, and ports. It depends on Domain.
- Infrastructure implements persistence and external integrations. Put EF Core mappings, migrations, and ePayco adapters here.
- API composes dependencies, authorizes requests, translates errors, and invokes use cases. Return API DTOs and keep business decisions in Domain and Application.
- Use asynchronous I/O, cancellation, pagination, and projections where relevant. Keep related state transitions consistent in transactions.

Organize Flutter by feature. Place app startup, routing, and theme in lib/app and truly shared infrastructure and components in lib/core. Keep domain contracts and use cases, data sources and repository implementations, and presentation widgets and controllers separate within each feature.
Widgets render state and send user actions to Riverpod controllers. Controllers invoke use cases and expose the resulting state. Keep business calculations and HTTP access in their own layers.
Keep every source file below 700 lines, including comments, blank lines, widgets, screens, tests, and migrations. The maximum is 699. Split by responsibility and keep formatting readable. Adjust generator inputs when generated code exceeds the limit and report a concrete blocker if the generator prevents compliance. Enforce the limit in project checks.

## Flutter interface for web and mobile

Design each screen once around the same use case and controller. Compose layouts from shared widgets using the available width. Centralize breakpoints, spacing, color, typography, and component states in the design system. Use layout constraints to choose compact, medium, and wide compositions.
- Compact screens use a single readable content column, accessible navigation, and reachable primary actions. Respect safe areas and keep inputs visible when the keyboard opens.
- Wider screens use side navigation, bounded reading widths, or list and detail panels where useful. Make tables and forms usable at narrower widths by wrapping or adapting content and keeping key actions visible.
- Keep navigation and deep links consistent. Handle browser refresh, back and forward navigation, keyboard focus, and tab order. Support touch, mouse, and keyboard where relevant.
- Use consistent components for forms, cards, status, and actions. Show status in text as well as color. Keep the primary action reachable and give icon-only actions accessible labels.
- Verify focus, contrast, text scaling, wrapping, and scroll behavior. Reserve space for long names, translated copy, large amounts, and validation messages.
- Define loading, empty, error, retry, offline, unauthorized, and success states for each data-driven screen. Preserve form data on recoverable errors and display messages next to the affected fields.
- Separate view states from transaction states. Disable repeat submission while a request is pending. Use the backend result to display payment, reservation, subscription, or delivery confirmation.
- Isolate platform capabilities behind adapters. Confirm camera, uploads, location, maps, and payment flows on the supported targets. Show an actionable fallback where a capability is unavailable. Web users can follow a trip and see the last authorized position. Enable location collection in the background on supported mobile devices after validation.
- Review each changed screen at compact and wide widths. Include long content, enlarged text, empty data, errors, and asynchronous loading. Validate navigation and the primary action end to end on web and mobile.

## Roles and authorization

| Role | Scope |
| --- | --- |
| Superadmin | Administers all application modules, users and roles, stores, subscriptions, foundations, publications, services, complaints, and approved financial actions. Require strong authentication and audit sensitive actions. |
| Business | Pays a monthly subscription to operate a store for pet supplies. Manages its own catalog, inventory, prices, orders, delivery coverage, and delivery service. Its own staff or chosen delivery provider fulfills its orders. |
| Foundation | Creates and manages its own adoption listings, requests, and adoption decisions. Can purchase a paid listing or visibility benefit under the advertised conditions. |

Customer and independent provider describe the operational profiles in the existing flows. Map their privileges and the three named roles to the approved model and role assignments.
Authorize every operation in the backend by role, permission, and resource scope. Superadmin has global application scope. Scope store and foundation data to their accounts and verify ownership or participation for customer and provider operations. Apply the same checks to lists, searches, downloads, and individual resources. Derive client capabilities from authorized state and use them to present relevant actions.
A confirmed and active monthly Business subscription enables storefront setup, publication, and sales. Define start, expiration, renewal, and cancellation from the confirmed payment and the accepted plan. Preserve access needed to fulfill and handle existing orders. Use the approved rules to decide which new actions require an active plan.
Record privileged actions with actor, action, target, timestamp, and relevant reason. Apply privacy and financial integrity checks to administrative operations.

## Product flows and screen requirements

### Adoption listings

Show basic listings and paid adoption listings with a clear label and the advertised visibility, price, duration, and moderation conditions. The publisher pays Mascotas for the listing or promotion. The foundation or authorized adopter contact handles the adoption process and decides the outcome. Keep the publication charge distinct from the adoption of the animal.
Show the listing, foundation or authorized publisher, and the application or contact action. Provide a separate management view for the foundation's own listings and requests.

### Stores and monthly subscriptions

Give Business a store dashboard with subscription status, current period, catalog, orders, and delivery settings. Publish merchant name, availability, product details, price, delivery cost, and expected delivery before checkout.
A checkout and its receipt identify the seller and delivery organizer. The store fulfills the sale and runs its own delivery, warranty, and product support. Mascotas handles its platform charges and the collection or support functions it undertakes.

### Pet transport

Capture pickup A, destination B, pet requirements, date and time, pickup and delivery contacts, and the customer's proposed budget. Providers can accept the proposal or submit counteroffers. Show inclusions, one-way or return arrangements, waiting conditions, total, and the provider the customer explicitly selects before confirmation. Show reservation or request, payment status, accepted offer, pickup, active trip, delivery, and cancellation consistently.
Share exact addresses and active location only with authorized participants. Start tracking from an explicit action for an active trip and stop on completion or cancellation. Show the last received position and time. Test locked screen, permission changes, loss of connectivity, retries, and recovery on supported mobile devices. Confirm pickup and delivery through the responsible participants.

### Bathing, washing, cleaning, and grooming

Support payment for pet bathing, washing, cleaning, grooming, and beauty at home or through a scheduled booking, according to the provider's offer. Display the service, location, provider, date and time, available slot, inclusions, total, and cancellation terms. For at-home service, use the service address.
Hold a slot according to the approved booking rules and reconcile the hold with confirmed payment. Handle expired holds, late payment confirmation, failure, cancellation, and refunds without duplicate reservations. Keep booking and payment states separately visible.

### Payments and small purchases

Show the complete total before confirmation, including platform charge, provider or seller amount, delivery, and applicable charges. Preserve the accepted price and the party responsible for fulfillment in the confirmation.
Use ePayco through a backend adapter. Validate provider notifications using the documented verification mechanism and reconcile amount, currency, reference, and operation. Treat redirections as navigation and use backend verification for the final result.
Use exact monetary representation with explicit rounding. Make subscriptions, listing activation, payments, wallet charges, refunds, and payouts idempotent. Protect balance and inventory operations against retries and concurrent requests.
Prioritize customer top-ups for several small purchases. Treat provider top-ups for commissions as a separate balance and charge each agreed fee once. Keep available, reserved, and spent balance distinct and separate company fees from money owed to sellers or providers. Apply the approved refund and payout rules and record evidence of changes.

## Security priority

Treat authorization, data protection, and payment integrity as acceptance criteria.
- Authenticate on the server and enforce least privilege on every resource. Validate object ownership and role changes, including direct requests made outside Flutter.
- Protect account recovery, administrative access, authentication endpoints, and payment notifications. Require stronger authentication for privileged users and revoke access when roles or sessions change.
- Store credentials in secure environment configuration. Use encrypted transport and restrict browser origins to the approved frontend origins.
- Validate input, uploads, filenames, and outgoing integrations. Store only necessary personal data. Protect identity documents, private addresses, location, and payment references with restricted access.
- Keep secrets and sensitive data out of code, responses, and logs. Record audit events and safe correlation references for investigations and refunds.

## Terms, privacy, and user consent

Provide versioned Terms and Conditions and a Privacy Policy on web and mobile. Make them accessible for reading and download from onboarding, checkout or booking, and account settings. Load the approved legal documents as resources with their effective date, version, and operator contact. Register the version, user, and date accepted in the backend according to the approved model. Collect authorization for personal data separately when required.
Explain the operator's paid platform functions and identify the independent seller, foundation, or provider responsible for each operation. Maintain the offerer's identification and notification details for authorized complaints and applicable authority requests. State the operator's own duties and preserve mandatory consumer and data rights. Describe subscription renewal, delivery, paid listings, service booking, charges, cancellations, applicable refunds, and tracked complaints.
Use the reviewed versions at docs/legal/terms-and-conditions.md and docs/legal/privacy-policy.md in the future project. Complete operator identity, support channels, effective dates, retention, and actual payment and data practices before publishing. Initial drafts for Colombia are stored in the ideas repository:
- Terms and conditions draft. https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/legal/terms-and-conditions.md
- Privacy policy draft. https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/legal/privacy-policy.md

## Required tests and delivery checks

Write unit tests in C# and Dart for new and changed behavior. Cover role and ownership rules, use cases, Riverpod state transitions, monetary calculations, subscription eligibility, listing activation, booking, cancellation, and duplicate notifications when touched.
Use API and PostgreSQL integration tests where a regression depends on authorization across resources, constraints, transactions, or concurrent payments. Add Flutter widget tests for changed responsive views, critical actions, error states, and text or layout overflow.
Before delivery, run the relevant dotnet build and test checks, dart format, flutter analyze, flutter test, and web and mobile builds available in the environment. Check the line limit, English code, centralized copy, and agreement with the entity relationship model. Record the checks performed and their actual outcomes.
Read the existing code and approved model before starting a task. Preserve user changes, implement the authorized slice, and report concrete blockers and remaining verification.
