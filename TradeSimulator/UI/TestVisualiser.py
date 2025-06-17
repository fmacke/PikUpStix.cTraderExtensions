import sys
import pandas as pd
import plotly.graph_objects as go
import plotly.subplots as sp
import locale
from sqlalchemy import create_engine



# Read command-line arguments
if len(sys.argv) < 5:
    print("Usage: python TestVisualiser.py <testId> <insId> <strategy> <runat> <saveFileTo>")
    sys.exit(1)

testId = sys.argv[1]
insId = sys.argv[2]
strategy = sys.argv[3] 
runat = sys.argv[4]
savefileTo = sys.argv[5] 

server = 'localhost'
database = 'TradingBE'
username = 'sa'
password = 'Gogogo123!'

# Set locale to UK for GBP formatting
locale.setlocale(locale.LC_ALL, 'en_GB.UTF-8')

# Establish connection to SQL Server
engine = create_engine(f"mssql+pyodbc://{username}:{password}@{server}/{database}?driver=ODBC+Driver+17+for+SQL+Server", fast_executemany=True)

# Fetch OHLC data
query_ohlc = """
SELECT Date, OpenPrice, HighPrice, LowPrice, ClosePrice
FROM HistoricalData
WHERE InstrumentId = ?
ORDER BY Date
"""
df_ohlc = pd.read_sql(query_ohlc, engine, params=(insId,))

# Fetch Positions data
query_positions = """
SELECT Id, Created, ClosedAt, EntryPrice, ClosePrice, PositionType, Margin
FROM Positions
WHERE TestId = ?
ORDER BY Created
"""

df_positions = pd.read_sql(query_positions, engine, params=(testId,))
# Close the connection
engine.dispose()

# Create subplots: (rows=2, columns=1)
fig = sp.make_subplots(rows=2, cols=1, shared_xaxes=True, vertical_spacing=0.1, 
                       subplot_titles=("OHLC Chart & Trades", "Margin Over Time"))

# Add OHLC candlestick chart (Top Graph)
fig.add_trace(go.Candlestick(
    x=df_ohlc['Date'],
    open=df_ohlc['OpenPrice'],
    high=df_ohlc['HighPrice'],
    low=df_ohlc['LowPrice'],
    close=df_ohlc['ClosePrice'],
    name="OHLC",
), row=1, col=1)

# Overlay trades (Top Graph)
for _, row in df_positions.iterrows():
    color = 'pink' if ((row['PositionType'] == 1 and row['ClosePrice'] > row['EntryPrice']) or 
                        (row['PositionType'] == 2 and row['ClosePrice'] < row['EntryPrice'])) else 'blue'
    
    tradeType = 'Buy' if row['PositionType'] == 1 else 'Sell'

    fig.add_trace(go.Scatter(
        x=[row['Created'], row['ClosedAt']],
        y=[row['EntryPrice'], row['ClosePrice']],
        mode='lines+markers',
        line=dict(color=color),
        name=f"{tradeType}, £{locale.format_string('%.2f', row['Margin'], grouping=True)}, {row['Id']}",
    ), row=1, col=1)



df_positions['CumulativeMargin'] = df_positions['Margin'].cumsum()

fig.add_trace(go.Scatter(
    x=df_positions['Created'], 
    y=df_positions['CumulativeMargin'],  # Use cumulative sum here
    mode='lines+markers', 
    name="Cumulative Margin Over Time",
    line=dict(color="green"),
), row=2, col=1)

# Layout adjustments
fig.update_layout(
    title=f"Trading Data: {strategy}, Instrument ID: {insId}, Run At: {runat}",
    xaxis_title="Date",
    xaxis_rangeslider_visible=False,
    showlegend=True
)

# Save to HTML file
html_output = f"{savefileTo}{testId}.html"
fig.write_html(html_output, include_plotlyjs='cdn')

print(f"Saved to {html_output}")