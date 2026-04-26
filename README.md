# SWE AutomationJs UI Design

This repository contains the Windows Forms restaurant management app for Automation of J's.

## Launching the app

The built executable is located at:

`SWE_AutomationJs\bin\Release\SWE AutomationJs UI Design.exe`

Keep the full `bin\Release` folder together when launching the app because the executable depends on the other files in that folder.

## Login credentials

Use one of the following seed accounts on the login screen:

- `E00001 / Admin@123`
- `E00002 / Manager@123`
- `E00003 / Server@123`
- `E00004 / Cashier@123`
- `E00005 / Kitchen@123`
- `E00006 / Server@123`
- `E00007 / Server@123`
- `E00008 / Kitchen@123`
- `E00009 / Kitchen@123`
- `E00010 / Busboy@123`

## Basic usage

### Admin / Manager

- Sign in with `E00001` or `E00002`.
- Use `Employee Records` to add, edit, view, or remove employee profiles.
- Use `Table Layout` to review table status and manage seats or merges.
- Use `Activity Log` to review login activity and other tracked actions.
- Use `Sales Reports` to review payment history and totals.

### Server

- Sign in with `E00003`, `E00006`, or `E00007`.
- Use `Assign Tables` to open tables and start orders.
- Add menu items, then submit the order to the kitchen.
- Return to the table after the kitchen marks the order ready.
- Use `Past Payments` to review paid orders.
- Use `FAQ` for restaurant contact information and operating hours.

### Kitchen

- Sign in with `E00005`, `E00008`, or `E00009`.
- Open incoming orders from the kitchen home screen.
- Review submitted orders and mark them ready when complete.

### Busboy

- Sign in with `E00010`.
- Open the cleaning queue.
- Mark dirty tables as available after they have been cleaned.

## Notes

- Table status colors indicate whether a table is open, occupied, or needs cleaning.
- Payment is available after the kitchen marks an order as ready.
- If the app does not start, make sure the SQLite database and DLL files remain beside the executable in the release folder.
