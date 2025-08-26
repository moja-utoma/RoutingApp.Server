-- -----------------------------
-- 15 Warehouses
-- -----------------------------
INSERT INTO [RoutingDB].[dbo].[Warehouses] ([Name], [Address], [Longitude], [Latitude], [IsDeleted], [DeletedAt])
VALUES
('Warehouse 1', '123 Main St', 10.123456, 50.123456, 0, NULL),
('Warehouse 2', '456 Oak St', 10.223456, 50.223456, 0, NULL),
('Warehouse 3', '789 Pine St', 10.323456, 50.323456, 0, NULL),
('Warehouse 4', '101 Maple Ave', 10.423456, 50.423456, 0, NULL),
('Warehouse 5', '202 Birch Ave', 10.523456, 50.523456, 0, NULL),
('Warehouse 6', '303 Cedar Rd', 10.623456, 50.623456, 0, NULL),
('Warehouse 7', '404 Elm Rd', 10.723456, 50.723456, 0, NULL),
('Warehouse 8', '505 Spruce St', 10.823456, 50.823456, 0, NULL),
('Warehouse 9', '606 Ash St', 10.923456, 50.923456, 0, NULL),
('Warehouse 10', '707 Willow St', 11.023456, 51.023456, 0, NULL),
('Warehouse 11', '808 Poplar St', 11.123456, 51.123456, 0, NULL),
('Warehouse 12', '909 Chestnut St', 11.223456, 51.223456, 0, NULL),
('Warehouse 13', '111 Walnut St', 11.323456, 51.323456, 0, NULL),
('Warehouse 14', '222 Fir St', 11.423456, 51.423456, 0, NULL),
('Warehouse 15', '333 Hickory St', 11.523456, 51.523456, 0, NULL);

-- -----------------------------
-- 50 Delivery Points
-- -----------------------------
INSERT INTO [RoutingDB].[dbo].[DeliveryPoints] ([Name], [Address], [Longitude], [Latitude], [Weight], [IsDeleted], [DeletedAt])
VALUES
('Delivery Point 1', '1 A St', 10.101, 50.101, 50, 0, NULL),
('Delivery Point 2', '2 B St', 10.102, 50.102, 75, 0, NULL),
('Delivery Point 3', '3 C St', 10.103, 50.103, 20, 0, NULL),
('Delivery Point 4', '4 D St', 10.104, 50.104, 100, 0, NULL),
('Delivery Point 5', '5 E St', 10.105, 50.105, 10, 0, NULL),
('Delivery Point 6', '6 F St', 10.106, 50.106, 60, 0, NULL),
('Delivery Point 7', '7 G St', 10.107, 50.107, 80, 0, NULL),
('Delivery Point 8', '8 H St', 10.108, 50.108, 30, 0, NULL),
('Delivery Point 9', '9 I St', 10.109, 50.109, 45, 0, NULL),
('Delivery Point 10', '10 J St', 10.110, 50.110, 55, 0, NULL),
('Delivery Point 11', '11 K St', 10.111, 50.111, 70, 0, NULL),
('Delivery Point 12', '12 L St', 10.112, 50.112, 15, 0, NULL),
('Delivery Point 13', '13 M St', 10.113, 50.113, 25, 0, NULL),
('Delivery Point 14', '14 N St', 10.114, 50.114, 90, 0, NULL),
('Delivery Point 15', '15 O St', 10.115, 50.115, 35, 0, NULL),
('Delivery Point 16', '16 P St', 10.116, 50.116, 40, 0, NULL),
('Delivery Point 17', '17 Q St', 10.117, 50.117, 60, 0, NULL),
('Delivery Point 18', '18 R St', 10.118, 50.118, 80, 0, NULL),
('Delivery Point 19', '19 S St', 10.119, 50.119, 15, 0, NULL),
('Delivery Point 20', '20 T St', 10.120, 50.120, 50, 0, NULL),
('Delivery Point 21', '21 U St', 10.121, 50.121, 35, 0, NULL),
('Delivery Point 22', '22 V St', 10.122, 50.122, 70, 0, NULL),
('Delivery Point 23', '23 W St', 10.123, 50.123, 25, 0, NULL),
('Delivery Point 24', '24 X St', 10.124, 50.124, 45, 0, NULL),
('Delivery Point 25', '25 Y St', 10.125, 50.125, 15, 0, NULL),
('Delivery Point 26', '26 Z St', 10.126, 50.126, 65, 0, NULL),
('Delivery Point 27', '27 AA St', 10.127, 50.127, 20, 0, NULL),
('Delivery Point 28', '28 BB St', 10.128, 50.128, 35, 0, NULL),
('Delivery Point 29', '29 CC St', 10.129, 50.129, 50, 0, NULL),
('Delivery Point 30', '30 DD St', 10.130, 50.130, 45, 0, NULL),
('Delivery Point 31', '31 EE St', 10.131, 50.131, 55, 0, NULL),
('Delivery Point 32', '32 FF St', 10.132, 50.132, 60, 0, NULL),
('Delivery Point 33', '33 GG St', 10.133, 50.133, 40, 0, NULL),
('Delivery Point 34', '34 HH St', 10.134, 50.134, 30, 0, NULL),
('Delivery Point 35', '35 II St', 10.135, 50.135, 90, 0, NULL),
('Delivery Point 36', '36 JJ St', 10.136, 50.136, 15, 0, NULL),
('Delivery Point 37', '37 KK St', 10.137, 50.137, 65, 0, NULL),
('Delivery Point 38', '38 LL St', 10.138, 50.138, 25, 0, NULL),
('Delivery Point 39', '39 MM St', 10.139, 50.139, 50, 0, NULL),
('Delivery Point 40', '40 NN St', 10.140, 50.140, 35, 0, NULL),
('Delivery Point 41', '41 OO St', 10.141, 50.141, 20, 0, NULL),
('Delivery Point 42', '42 PP St', 10.142, 50.142, 45, 0, NULL),
('Delivery Point 43', '43 QQ St', 10.143, 50.143, 60, 0, NULL),
('Delivery Point 44', '44 RR St', 10.144, 50.144, 70, 0, NULL),
('Delivery Point 45', '45 SS St', 10.145, 50.145, 30, 0, NULL),
('Delivery Point 46', '46 TT St', 10.146, 50.146, 40, 0, NULL),
('Delivery Point 47', '47 UU St', 10.147, 50.147, 15, 0, NULL),
('Delivery Point 48', '48 VV St', 10.148, 50.148, 35, 0, NULL),
('Delivery Point 49', '49 WW St', 10.149, 50.149, 50, 0, NULL),
('Delivery Point 50', '50 XX St', 10.150, 50.150, 55, 0, NULL);

-- -----------------------------
-- 30 Vehicles
-- -----------------------------
INSERT INTO [RoutingDB].[dbo].[Vehicles] ([Name], [Capacity], [WarehouseId], [IsDeleted], [DeletedAt])
VALUES
('Vehicle 1', 100, 1, 0, NULL),
('Vehicle 2', 120, 2, 0, NULL),
('Vehicle 3', 80, 1, 0, NULL),
('Vehicle 4', 150, 3, 0, NULL),
('Vehicle 5', 200, 4, 0, NULL),
('Vehicle 6', 100, 5, 0, NULL),
('Vehicle 7', 120, 6, 0, NULL),
('Vehicle 8', 90, 7, 0, NULL),
('Vehicle 9', 110, 8, 0, NULL),
('Vehicle 10', 130, 9, 0, NULL),
('Vehicle 11', 100, 10, 0, NULL),
('Vehicle 12', 90, 11, 0, NULL),
('Vehicle 13', 140, 12, 0, NULL),
('Vehicle 14', 150, 13, 0, NULL),
('Vehicle 15', 160, 14, 0, NULL),
('Vehicle 16', 120, 15, 0, NULL),
('Vehicle 17', 130, 1, 0, NULL),
('Vehicle 18', 110, 2, 0, NULL),
('Vehicle 19', 100, 3, 0, NULL),
('Vehicle 20', 140, 4, 0, NULL),
('Vehicle 21', 150, 5, 0, NULL),
('Vehicle 22', 120, 6, 0, NULL),
('Vehicle 23', 130, 7, 0, NULL),
('Vehicle 24', 110, 8, 0, NULL),
('Vehicle 25', 100, 9, 0, NULL),
('Vehicle 26', 140, 10, 0, NULL),
('Vehicle 27', 150, 11, 0, NULL),
('Vehicle 28', 120, 12, 0, NULL),
('Vehicle 29', 130, 13, 0, NULL),
('Vehicle 30', 110, 14, 0, NULL);

-- -----------------------------
-- 10 Routes
-- -----------------------------
INSERT INTO [RoutingDB].[dbo].[Routes] ([Name], [IsDeleted], [DeletedAt])
VALUES
('Route 1', 0, NULL),
('Route 2', 0, NULL),
('Route 3', 0, NULL),
('Route 4', 0, NULL),
('Route 5', 0, NULL),
('Route 6', 0, NULL),
('Route 7', 0, NULL),
('Route 8', 0, NULL),
('Route 9', 0, NULL),
('Route 10', 0, NULL);

-- -----------------------------
-- Route <-> DeliveryPoints
-- -----------------------------
INSERT INTO [RoutingDB].[dbo].[DeliveryPointRoute] ([DeliveryPointsId], [RoutesId])
VALUES
(1,1),(2,1),(3,1),(4,2),(5,2),(6,2),(7,3),(8,3),(9,3),(10,4),
(11,4),(12,4),(13,5),(14,5),(15,5),(16,6),(17,6),(18,6),(19,7),(20,7),
(21,7),(22,8),(23,8),(24,8),(25,9),(26,9),(27,9),(28,10),(29,10),(30,10);

-- -----------------------------
-- Route <-> Warehouses
-- -----------------------------
INSERT INTO [RoutingDB].[dbo].[RouteWarehouse] ([RoutesId], [WarehousesId])
VALUES
(1,1),(1,2),(2,3),(2,4),(3,5),(3,6),(4,7),(4,8),(5,9),(5,10),
(6,11),(6,12),(7,13),(7,14),(8,15),(9,1),(9,2),(10,3),(10,4),(10,5);
