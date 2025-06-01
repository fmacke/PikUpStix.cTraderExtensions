import sys
import pandas as pd
import pyodbc
import plotly.graph_objects as go
import locale

# Read command-line arguments
if len(sys.argv) < 5:
    print("Usage: python TestVisualiser.py <testId> <insId> <strategy> <runat>")
    sys.exit(1)

testId = sys.argv[1]
insId = sys.argv[2]
strategy = sys.argv[3] 
runat = sys.argv[4]

server = 'localhost'
database = 'TradingBE'
username = 'sa'
password = 'Gogogo123!'

# Set locale to UK for GBP formatting
locale.setlocale(locale.LC_ALL, 'en_GB.UTF-8')

# Establish connection to SQL Server
conn = pyodbc.connect(f'DRIVER={{ODBC Driver 17 for SQL Server}};SERVER={server};DATABASE={database};UID={username};PWD={password}')

# Fetch OHLC data
query_ohlc = """
SELECT Date, OpenPrice, HighPrice, LowPrice, ClosePrice
FROM HistoricalData
WHERE InstrumentId = ?
ORDER BY Date
"""
df_ohlc = pd.read_sql(query_ohlc, conn, params=[insId])

# Fetch Positions data
query_positions = """
SELECT Id, Created, ClosedAt, EntryPrice, ClosePrice, PositionType, Margin
FROM Positions
WHERE TestId = ?
ORDER BY Created
"""
df_positions = pd.read_sql(query_positions, conn, params=[testId])

# Close the connection
conn.close()

# Create OHLC chart with dynamic y-axis range
fig = go.Figure(data=[go.Ohlc(x=df_ohlc['Date'],
                              open=df_ohlc['OpenPrice'],
                              high=df_ohlc['HighPrice'],
                              low=df_ohlc['LowPrice'],
                              close=df_ohlc['ClosePrice'])])

# Overlay trades from Positions table with color-coding based on profit/loss
for index, row in df_positions.iterrows():
    color = 'gray'  # Default color to avoid NameError
    if row['PositionType'] == 1 and row['ClosePrice'] > row['EntryPrice']:
     color = 'pink'
    elif row['PositionType'] == 1 and row['ClosePrice'] < row['EntryPrice']:
     color = 'blue'
    elif row['PositionType'] == 2 and row['ClosePrice'] < row['EntryPrice']:
     color = 'pink'
    elif row['PositionType'] == 2 and row['ClosePrice'] > row['EntryPrice']:
     color = 'blue'

    tradeType = 'Buy' 
    if row['PositionType'] == 2:
      tradeType = 'Sell'

    fig.add_trace(go.Scatter(
        x=[row['Created'], row['ClosedAt']],
        y=[row['EntryPrice'], row['ClosePrice']],
        mode='lines+markers',
        line=dict(color=color),
        name=f"{tradeType}{', '}{f'£{locale.format_string("%.2f", row['Margin'], grouping=True)}'}{', '}{row['Id']}"
    ))

formatted_amount = f'£{locale.format_string("%.2f", df_positions["Margin"].sum(), grouping=True)}'


fig.update_layout(
    title=strategy + ', Instrument ID: ' + insId + ', Run At: ' + runat + ', Margin Sum: ' + formatted_amount,
    xaxis_title='Date',
    yaxis_title='Price',
    xaxis_rangeslider_visible=True,
    yaxis=dict(autorange=True)
)

# JavaScript to dynamically update y-axis range
fig.update_layout(
    xaxis=dict(
        rangeselector=dict(
            buttons=list([
                dict(count=1, label="1m", step="month", stepmode="backward"),
                dict(count=6, label="6m", step="month", stepmode="backward"),
                dict(step="all")
            ])
        ),
        rangeslider=dict(visible=True),
        type="date"
    ),
    yaxis=dict(fixedrange=False)
)

# Save as HTML
fig.write_html(f"C:\\Users\\finn\\OneDrive\\Documents\\Money\\Business\\trading\\TestExports\\{testId}.html", include_plotlyjs='cdn')
print(f"{testId}.html")