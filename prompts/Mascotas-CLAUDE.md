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

## Release plan and roles

Ship the adoption release first. Use the work plan for dependencies, acceptance checks, release gates, and the Colombia pilot. Follow the approved entity relationship model when implementing these requirements. [Mascotas work plan](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/work-plan.md)

| Role | Scope |
| --- | --- |
| User | Manages their personal account and adoption listings. Can buy paid adoption visibility and use the available customer flows. |
| Business | Manages an authorized organization profile, membership, and its own resources. Can publish paid adoption listings and enable the approved store, service, or transport capabilities as those phases open. |
| Superadmin | Administers all application modules, including users, organization enrollment, moderation, prices, terms, and authorized financial actions. Can publish and pay for its own adoption listing. Require strong authentication and audit sensitive actions. |

Use one Business role for both Foundation and Store profile types. A Business can operate without a store. Authorize role, capability, and resource scope in the backend for every operation, including lists, searches, downloads, and direct resource access. Scope Business operations to the authorized organization and preserve account and operation history during profile conversion. Audit sensitive actions with actor, action, target, timestamp, and reason.

## Phase 1 — Adoptions

### Registration, locations, and Business enrollment

Ask for name, email, password, country, and city at personal registration. Preselect Colombia and enable it as the initial country. Use the approved location catalog to support several cities from launch and future countries through configuration. Group Colombian locations by department in the selector. Keep official DIVIPOLA codes as text and validate country, department, and location on the server. Store a listing's location independently from the account's city. [DANE catalog and import notes](https://github.com/erickvalderrama/it/tree/main/prompts/mascotas/data)

Start Business enrollment with a choice of store or foundation for both new accounts and conversions of existing accounts. Request store identity evidence when the person enrolls as a store or later enables a storefront. For a natural person request a Colombian identity card and RUT. For a legal entity request NIT and RUT, with representation checks where applicable. Keep these documents private, restrict verification access, and minimize retention. Apply the same Business membership rules to both profile types.

Support free promotional enrollment for eligible foundations and future stores with configurable eligibility, start, and end dates. A free year from Business activation is a pilot proposal. Offer the assigned monthly price and require explicit acceptance and confirmed payment at renewal. Show status, benefits, expiry, and next price in the Business profile. Start with manual monthly renewal as proposed in the plan. Honor existing obligations after membership expiry.

### Adoption listing, filters, and review

Provide basic and paid adoption listings. User, Business, and Superadmin can buy paid listings. Record whether the publisher acted as an individual or for an organization at publication time. Preserve that source after role or profile changes and filter public results by city and publisher type. Show the publisher, pet profile, adoption contact or application action, and listing status. Allow publishers to manage their own listings and adoption requests. The authorized publisher decides the adoption and handles the animal's handover.

Show the fee, benefits, duration, content rules, and refund conditions for the paid listing. Mascotas receives the charge for the listing or its visibility. The publisher handles the adoption separately. For a paid listing, charge first and submit its exact revision for review after server-side ePayco confirmation. Make the listing visible and start the paid period only after approval. Track review, publication, payment, invoicing, and refund as separate states.

Use an approved multimodal AI provider such as OpenAI API behind a backend port. Review the listing text, each image, and visible text within images. Send only the necessary sanitized content and strip image metadata. Use approved moderation rules and structured, validated assessments. Verify policy coverage for each input type and require a result for every submitted image. Keep identity documents, billing data, and private contact details outside the review request.

- Approve automatically when all required text and images pass the rules.
- Request corrections for fixable issues. Identify the field or photo and explain how to fix it. Let the publisher revise and resubmit the same paid listing using the original payment and accepted price.
- Reject the submitted revision for clear vulgarity, any sexual content in text or images, or another clear policy violation. Keep the revision hidden and tell the publisher why. Allow a clean replacement where the policy permits it.
- Route uncertainty, unreadable or missing image review, inconsistent responses, and persistent provider failure to Superadmin. Record the decision and its reason.

Re-review all content in each submitted revision. Keep unapproved new listings private and prevent stale review results from activating another revision. Record content revision, policy version, assessment by input, provider, outcome, and human decision. Minimize retention of prohibited material and keep it out of notifications. The pilot proposes a full refund for final rejection before activation. Confirm supported refund methods and publish the correction and refund terms before charging. Link each refund and applicable invoice adjustment to the original transaction and apply it once.

### Versioned prices, ePayco, and electronic invoicing

Keep a dedicated history of immutable price versions, with amounts, currency, product or membership scope, effective dates, and assigned Business where applicable. Create a new version when a price changes. Record the exact version, accepted total, currency, applicable benefits, timestamp, and actor for each publication or payment, including zero-price offers. Link the listing, payment, membership period, and invoice to the corresponding accepted offer. Support querying the latest accepted price while retaining every prior acceptance. Validate and freeze an accepted quote server-side for its stated lifetime. Resolve quote expiry and late payment confirmations explicitly.

Use ePayco for listing and Business charges through a backend adapter. Verify each notification using the provider's documented mechanism and reconcile amount, currency, reference, and operation. Use the authenticated server result for the final status. Apply a confirmation once even if it arrives repeatedly or concurrently.

For named invoices, ask for the permitted buyer identification data at checkout and offer the applicable final-consumer flow. Keep phone and address separate when needed for an operational purpose and apply current DIAN requirements to invoice issuance. Issue electronic invoices for Mascotas listing and membership charges through the approved DIAN-enabled flow. Keep the ePayco payment confirmation and electronic invoice linked with separate identifiers and states. Record issuance, validation, delivery, retries, and applicable corrections. Use an authorized refund flow when required.

The pilot proposal starts paid listings and memberships on responsive web. Review Apple and Google Play rules for digital purchases before enabling them in store-distributed mobile apps. Treat physical service and product checkout as a separate review.

### Phase 1 screens and administration

Deliver registration, city and publisher filters, a shareable listing detail, listing draft and preview, review and payment status, Business enrollment and membership, terms acceptance, invoice access, and a Superadmin dashboard. Reuse the same states and controls on compact and wide layouts. Show registered, enabled, and active user counts, filterable by city, role, and period. Display the definition and period of the active user count. Include review, Business enrollment, pricing, terms versions, and payment or invoice recovery in the dashboard. Keep administrative capabilities audited.

## Phase 2 — Stores and pet services

Give a verified Business a store dashboard with subscription status, catalog, stock, orders, and its own delivery settings. Publish merchant name, availability, product details, price, delivery cost, and expected delivery before checkout. The store fulfills the order and handles delivery and product support. Mascotas handles the platform charges and functions it undertakes. Preserve access needed to fulfill orders already accepted.

Support paid pet bathing, washing, cleaning, grooming, and beauty at home or through a scheduled booking. Display service, location, provider, date, time slot, inclusions, total, and cancellation terms. Reconcile slot holds with confirmed payment. Handle expiry, late confirmations, failure, cancellation, and refunds with consistent booking and payment states.

Integrate ePayco Payouts for eligible verified beneficiaries and reconcile each payout with the underlying operation. Show the complete total and the party responsible for fulfillment before confirmation. Use exact monetary representation, explicit rounding, and idempotent payment, refund, inventory, balance, and payout operations.

Support customer top-ups for eligible small purchases and separate Business top-ups for commissions. Keep available, reserved, and consumed balance and provider payables traceable. Apply each agreed fee once. Obtain approval for the custody, terms, billing, refund, and provider rules before enabling balances.

## Phase 3 — Pet transport from A to B

Add the transport capability to Business after applicable operator, vehicle, and coverage verification. Before paid trips, resolve the Colombian transport modality and authorization for the actual service, contract, animal welfare and custody, insurance coverage, and incident protocol.

Capture pickup A, destination B, pet requirements, time, pickup and delivery contacts, and the customer's proposed budget. Providers can accept the proposal or submit counteroffers. Show inclusions, one-way or return arrangements, waiting conditions, price, and the provider selected by the customer. Show request, accepted offer, payment, pickup, active trip, delivery, and cancellation consistently. Confirm pickup and delivery with the responsible participants.

Share exact addresses and active location with authorized participants. Start tracking from an explicit action during an active trip and stop on completion or cancellation. Show the last received position and time. Test locked screens, permission changes, loss of connectivity, retries, and recovery on supported mobile devices.

Reconcile the trip charge, platform commission, provider amount, payout, and refund where applicable. Support approved small-payment or commission top-ups with separate, auditable balances.

## Security priority

Treat authorization, data protection, and payment integrity as acceptance criteria.
- Authenticate on the server and enforce least privilege on every resource. Validate object ownership and role changes, including direct requests made outside Flutter.
- Protect account recovery, administrative access, authentication endpoints, and payment notifications. Require stronger authentication for privileged users and revoke access when roles or sessions change.
- Store credentials in secure environment configuration. Use encrypted transport and restrict browser origins to the approved frontend origins.
- Validate input, uploads, filenames, and outgoing integrations. Store only necessary personal data. Protect identity documents, private addresses, location, and payment references with restricted access.
- Keep secrets and sensitive data out of code, responses, and logs. Record audit events and safe correlation references for investigations and refunds.

## Terms, privacy, and user consent

Keep immutable versions of the Terms and Conditions and Privacy Policy with language, text, version, and effective date. Make them accessible for reading and download from onboarding, relevant transactions, and account settings. Record the exact document version, user, and acceptance time in the backend. Preserve the full history and the latest accepted version for each document. Collect personal data authorization separately when required.

When updated Terms apply, present them at sign-in or before a protected flow. Require explicit acceptance and enforce it server-side before new publications and payments. Preserve access to legal documents, existing operations, and support. Associate each accepted operation with the legal version applicable to it.

In the legal copy, specify that Mascotas provides the advertised digital tools and is responsible for the functions and charges it undertakes. The publisher manages and is responsible for the adoption information and handover, the store handles its sale and delivery, and the independent provider performs the agreed service. Identify the responsible party in each flow and preserve mandatory consumer and data rights. Describe AI-assisted moderation, membership pricing, renewals, complaints, cancellations, and refunds as they apply to the enabled features.

Use the reviewed versions at docs/legal/terms-and-conditions.md and docs/legal/privacy-policy.md in the future project. Complete operator identity, support channels, effective dates, retention, and actual payment and data practices before publishing. Use the initial Colombia drafts for legal review.
- [Terms and conditions draft](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/legal/terms-and-conditions.md)
- [Privacy policy draft](https://github.com/erickvalderrama/it/blob/main/prompts/mascotas/legal/privacy-policy.md)

## Required tests and delivery checks

Write unit tests in C# and Dart for new and changed behavior. Cover role and ownership rules, use cases, Riverpod state transitions, monetary calculations, membership eligibility, location validation, per-image and text moderation, sexual and vulgar content rejection, corrections with the original payment, price and Terms version changes, listing activation, refunds, invoice retry, booking, cancellation, and duplicate notifications when touched.
Use API and PostgreSQL integration tests where a regression depends on authorization across resources, constraints, transactions, or concurrent payments. Add Flutter widget tests for changed responsive views, critical actions, error states, and text or layout overflow.
Before delivery, run the relevant dotnet build and test checks, dart format, flutter analyze, flutter test, and web and mobile builds available in the environment. Check the line limit, English code, centralized copy, and agreement with the entity relationship model. Record the checks performed and their actual outcomes.
Read the existing code and approved model before starting a task. Preserve user changes, implement the authorized slice, and report concrete blockers and remaining verification.
